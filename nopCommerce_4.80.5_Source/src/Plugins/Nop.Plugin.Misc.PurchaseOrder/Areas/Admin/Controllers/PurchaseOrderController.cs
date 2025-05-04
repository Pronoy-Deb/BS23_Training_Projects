using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Catalog;
using Nop.Core;
using Nop.Services.Messages;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class PurchaseOrderController : BasePluginController
    {
        private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
        private readonly IProductSupplierService _productSupplierService;
        private readonly ISuppliersService _suppliersService;
        private readonly IRepository<PurchaseOrderRecord> _purchaseOrderRepository;
        private readonly IProductService _productService;
        private readonly IPurchaseOrderSupplierService _purchaseOrderSupplierService;
        private readonly IPictureService _pictureService;
        private readonly IRepository<PurchaseOrderProductRecord> _purchaseOrderProductRepository;
        private readonly MediaSettings _mediaSettings;
        private readonly INotificationService _notificationService;
        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IProductSupplierService productSupplierService,
            ISuppliersService suppliersService,
            IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IProductService productService,
            IPurchaseOrderSupplierService purchaseOrderSupplierService,
            IPictureService pictureService,
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository,
            MediaSettings mediaSettings,
            INotificationService notificationService)
        {
            _purchaseOrderModelFactory = purchaseOrderModelFactory;
            _productSupplierService = productSupplierService;
            _suppliersService = suppliersService;
            _purchaseOrderRepository = purchaseOrderRepository;
            _productService = productService;
            _purchaseOrderSupplierService = purchaseOrderSupplierService;
            _pictureService = pictureService;
            _purchaseOrderProductRepository = purchaseOrderProductRepository;
            _mediaSettings = mediaSettings;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> List()
        {
            var searchModel = new PurchaseOrderSearchModel();
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderSearchModelAsync(searchModel);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(PurchaseOrderSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderListModelAsync(searchModel);
            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderCreateModelAsync(new PurchaseOrderCreateModel(), null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data" });
            }

            try
            {
                var supplier = await _suppliersService.GetByIdAsync(model.SelectedSupplierId);
                if (supplier == null)
                {
                    return Json(new { success = false, message = "Selected supplier not found" });
                }

                var totalAmount = model.SelectedProducts?.Sum(p => p.QuantityToOrder * p.UnitCost) ?? 0;

                var order = new PurchaseOrderRecord
                {
                    OrderDate = DateTime.Now,
                    SupplierId = model.SelectedSupplierId,
                    SupplierName = supplier.Name,
                    TotalAmount = totalAmount,
                };

                await _purchaseOrderRepository.InsertAsync(order);

                if (model.SelectedProducts != null)
                {
                    foreach (var productModel in model.SelectedProducts)
                    {
                        var product = await _productService.GetProductByIdAsync(productModel.ProductId);
                        if (product == null)
                            continue;

                        product.StockQuantity += productModel.QuantityToOrder;
                        await _productService.UpdateProductAsync(product);

                        var picture = (await _productService.GetProductPicturesByProductIdAsync(productModel.ProductId)).FirstOrDefault();
                        var pictureUrl = await _pictureService.GetPictureUrlAsync(picture?.PictureId ?? 0);

                        var orderProduct = new PurchaseOrderProductRecord
                        {
                            PurchaseOrderId = order.Id,
                            ProductId = productModel.ProductId,
                            Quantity = productModel.QuantityToOrder,
                            UnitPrice = productModel.UnitCost,
                            ProductName = productModel.ProductName ?? product.Name,
                            ProductSku = productModel.ProductSku ?? product.Sku,
                            PictureThumbnailUrl = pictureUrl,
                        };

                        await _purchaseOrderProductRepository.InsertAsync(orderProduct);
                    }
                }
                _notificationService.SuccessNotification("The order is successfully placed");

                return Json(new
                {
                    success = true,
                    redirect = Url.Action("List")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while creating the order. Please try again."
                });
            }
        }

        public async Task<IActionResult> ProductAddPopup(int purchaseOrderId, string btnId, string formId)
        {
            var model = await _purchaseOrderModelFactory.PrepareAddProductToPurchaseOrderSearchModelAsync(new AddProductToPurchaseOrderSearchModel
            {
                PurchaseOrderId = purchaseOrderId
            });

            ViewBag.ButtonId = btnId;
            ViewBag.FormId = formId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductAddPopup(AddProductToPurchaseOrderSearchModel searchModel, int supplierId)
        {
            var filteredProducts = await _productSupplierService.GetProductsBySupplierIdAsync(supplierId);

            var products = await _purchaseOrderSupplierService.SearchProductsBySupplierAsync(
                supplierId: supplierId,
                products: filteredProducts,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            var model = await ModelExtensions.PrepareToGridAsync(
                new ProductListModel(),
                searchModel,
                products,
                () => TransformProductsAsync(products));

            return Json(model);
        }

        private async IAsyncEnumerable<ProductModel> TransformProductsAsync(IPagedList<Product> products)
        {
            foreach (var p in products)
            {
                var productModel = p.ToModel<ProductModel>();
                productModel.StockQuantity = p.StockQuantity;

                var picture = (await _productService.GetProductPicturesByProductIdAsync(p.Id)).FirstOrDefault();
                productModel.PictureThumbnailUrl = await _pictureService.GetPictureUrlAsync(
                    picture?.PictureId ?? 0,
                    _mediaSettings.ProductThumbPictureSize);

                yield return productModel;
            }
        }

        public async Task<IActionResult> ViewSnapshot(int id)
        {
            var order = await _purchaseOrderRepository.Table
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var orderProducts = await _purchaseOrderProductRepository.Table
                .Where(p => p.PurchaseOrderId == id)
                .ToListAsync();

            var supplierName = order.SupplierName;
            if (string.IsNullOrEmpty(supplierName))
            {
                var supplier = await _suppliersService.GetByIdAsync(order.SupplierId);
                supplierName = supplier?.Name ?? "Supplier Not Found";
            }

            var model = new PurchaseOrderSnapshotModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                SupplierName = supplierName,
                TotalAmount = order.TotalAmount,
                CreatedBy = "Admin",
                Products = orderProducts.Select(p => new PurchaseOrderSnapshotModel.OrderProductSnapshot
                {
                    ProductName = p.ProductName ?? "Unknown Product",
                    ProductSku = p.ProductSku ?? "N/A",
                    PictureThumbnailUrl = p.PictureThumbnailUrl ?? "/images/default-image.png",
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice
                }).ToList()
            };
            return View(model);
        }
    }
}
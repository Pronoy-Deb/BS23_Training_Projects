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
using Microsoft.Extensions.FileProviders;

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
        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IProductSupplierService productSupplierService,
            ISuppliersService suppliersService,
            IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IProductService productService,
            IPurchaseOrderSupplierService purchaseOrderSupplierService,
            IPictureService pictureService,
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository,
            MediaSettings mediaSettings)
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
                        if (productModel.QuantityToOrder > product.StockQuantity)
                        {
                            return Json(new
                            {
                                success = false,
                                message = $"Quantity for {product.Name} exceeds available stock ({product.StockQuantity})"
                            });
                        }

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
        [FormValueRequired("save")]
        public async Task<IActionResult> ProductAddPopup(AddProductToPurchaseOrderModel model)
        {
            if (model.SelectedProductIds?.Any() != true)
                return Json(new { success = false, message = "No products selected" });

            var products = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            var selectedProducts = new List<ProductSelectionModel>();

            foreach (var product in products)
            {
                var productPictures = await _productService.GetProductPicturesByProductIdAsync(product.Id);
                var productPicture = productPictures.FirstOrDefault();

                Picture picture = null;
                if (productPicture != null)
                    picture = await _pictureService.GetPictureByIdAsync(productPicture.PictureId);

                string thumbnailUrl;
                if (picture != null)
                {
                    thumbnailUrl = (await _pictureService.GetPictureUrlAsync(
                        picture,
                        storeLocation: $"{Request.Scheme}://{Request.Host}"
                    )).Url;
                }
                else
                {
                    thumbnailUrl = $"{Request.Scheme}://{Request.Host}/images/default-image.png";
                }

                selectedProducts.Add(new ProductSelectionModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductSku = product.Sku,
                    PictureThumbnailUrl = thumbnailUrl,
                    QuantityToOrder = 1,
                    UnitCost = product.Price,
                    StockQuantity = product.StockQuantity,
                    Selected = true
                });
            }

            return Json(new { success = true, products = selectedProducts });
        }

        [HttpPost]
        public async Task<IActionResult> ProductAddPopupList(AddProductToPurchaseOrderSearchModel searchModel, int supplierId)
        {
            // Get product IDs assigned to the supplier
            var filteredProducts = await _productSupplierService.GetProductsBySupplierIdAsync(supplierId);

            // Now filter the products based on those IDs and other search filters
            var products = await _purchaseOrderSupplierService.SearchProductsBySupplierAsync(
                supplierId: supplierId,
                products: filteredProducts,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            // Map to model
            var model = new ProductListModel().PrepareToGrid(searchModel, products, () =>
            {
                return products.Select(p =>
                {
                    var productModel = p.ToModel<ProductModel>();
                    // Perform any additional custom mapping if needed
                    return productModel;
                });
            });

            return Json(model);
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
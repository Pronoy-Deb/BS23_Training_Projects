using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.PurchaseOrder.ExportImport;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Security;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class PurchaseOrderController : BasePluginController
    {
        private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderExportManager _exportManager;
        private readonly IProductSupplierService _productSupplierService;
        private readonly IPermissionService _permissionService;
        private readonly ISuppliersService _suppliersService;
        private readonly IRepository<PurchaseOrderRecord> _purchaseOrderRepository;
        private readonly IProductService _productService;
        private readonly IPurchaseOrderSupplierService _purchaseOrderSupplierService;
        private readonly IPictureService _pictureService;
        private readonly IRepository<PurchaseOrderProductRecord> _purchaseOrderProductRepository;
        private readonly IPriceFormatter _priceFormatter;

        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderExportManager exportManager,
            IProductSupplierService productSupplierService,
            IPermissionService permissionService,
            ISuppliersService suppliersService,
            IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IProductService productService,
            IPurchaseOrderSupplierService purchaseOrderSupplierService,
            IPictureService pictureService,
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository,
            IPriceFormatter priceFormatter)
        {
            _purchaseOrderModelFactory = purchaseOrderModelFactory;
            _purchaseOrderService = purchaseOrderService;
            _exportManager = exportManager;
            _productSupplierService = productSupplierService;
            _permissionService = permissionService;
            _suppliersService = suppliersService;
            _purchaseOrderRepository = purchaseOrderRepository;
            _productService = productService;
            _purchaseOrderSupplierService = purchaseOrderSupplierService;
            _pictureService = pictureService;
            _purchaseOrderProductRepository = purchaseOrderProductRepository;
            _priceFormatter = priceFormatter;
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
                // Get supplier details
                var supplier = await _suppliersService.GetByIdAsync(model.SelectedSupplierId);
                if (supplier == null)
                {
                    return Json(new { success = false, message = "Selected supplier not found" });
                }

                // Calculate total amount
                var totalAmount = model.SelectedProducts?.Sum(p => p.QuantityToOrder * p.UnitCost) ?? 0;

                // Create and save the purchase order
                var order = new PurchaseOrderRecord
                {
                    OrderDate = DateTime.UtcNow,
                    SupplierId = model.SelectedSupplierId,
                    SupplierName = supplier.Name,
                    TotalAmount = totalAmount,
                };

                await _purchaseOrderRepository.InsertAsync(order);

                // Save each product if they exist
                if (model.SelectedProducts != null)
                {
                    foreach (var productModel in model.SelectedProducts)
                    {
                        var product = await _productService.GetProductByIdAsync(productModel.ProductId);
                        if (product == null)
                            continue;

                        // Get product picture
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
            {
                return Json(new { success = false, message = "No products selected" });
            }

            var products = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            var selectedProducts = new List<object>();

            foreach (var product in products)
            {
                var pictureId = (await _productService.GetProductPicturesByProductIdAsync(product.Id)).FirstOrDefault()?.PictureId ?? 0;
                selectedProducts.Add(new
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductSku = product.Sku,
                    PictureThumbnailUrl = await _pictureService.GetPictureUrlAsync(pictureId),
                    UnitCost = product.Price,
                    QuantityToOrder = 1,
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

        [HttpPost]
        public async Task<IActionResult> GetSelectedProductDetails(List<int> selectedIds)
        {
            var products = new List<ProductSelectionModel>();

            foreach (var id in selectedIds)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    // Get product pictures
                    var productPictures = await _productService.GetProductPicturesByProductIdAsync(product.Id);
                    var firstPicture = productPictures.FirstOrDefault();
                    var pictureUrl = firstPicture != null
                        ? await _pictureService.GetPictureUrlAsync(firstPicture.PictureId, 75)
                        : string.Empty;

                    products.Add(new ProductSelectionModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductSku = product.Sku,
                        PictureThumbnailUrl = pictureUrl,
                        QuantityToOrder = 1, // Default quantity
                        UnitCost = product.Price, // Default to product price
                        Selected = true
                    });
                }
            }

            return Json(new { success = true, products = products });
        }

        public async Task<IActionResult> ViewSnapshot(int id)
        {
            // 1. First get the basic order info
            var order = await _purchaseOrderRepository.Table
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            // 2. Get all products for this order
            var orderProducts = await _purchaseOrderProductRepository.Table
                .Where(p => p.PurchaseOrderId == id)
                .ToListAsync();

            // 3. Get supplier name (from either snapshot or current supplier table)
            var supplierName = order.SupplierName;
            if (string.IsNullOrEmpty(supplierName))
            {
                var supplier = await _suppliersService.GetByIdAsync(order.SupplierId);
                supplierName = supplier?.Name ?? "Supplier Not Found";
            }

            // 4. Create the view model
            var model = new PurchaseOrderSnapshotModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                SupplierName = supplierName,
                TotalAmount = order.TotalAmount,
                CreatedBy = "Admin", // Default value
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

        [HttpPost]
        public virtual async Task<IActionResult> ExportCsv(PurchaseOrderSearchModel searchModel)
        {
            var pagedOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync(searchModel);

            var bytes = _exportManager.ExportPurchaseOrdersToCsv(pagedOrders.ToList());

            return File(bytes, MimeTypes.TextCsv, "purchaseorders.csv");
        }
    }
}
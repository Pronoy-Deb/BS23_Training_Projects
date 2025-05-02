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
        private readonly ISuppliersService _supplierService;
        private readonly IPictureService _pictureService;

        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderExportManager exportManager,
            IProductSupplierService productSupplierService,
            IPermissionService permissionService,
            ISuppliersService suppliersService,
            IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IProductService productService,
            IPurchaseOrderSupplierService purchaseOrderSupplierService,
            ISuppliersService supplierService,
            IPictureService pictureService)
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
            _supplierService = supplierService;
            _pictureService = pictureService;
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
        public async Task<IActionResult> Create(PurchaseOrderCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSuppliers = await _supplierService.GetAllSuppliersForDropdownAsync();
                return View(model);
            }

            // Get selected products from form data
            var selectedProducts = new List<OrderProductItem>();
            foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("SelectedProducts[")))
            {
                if (key.Contains("].Selected") && Request.Form[key] == "true")
                {
                    var productIdStr = key.Split('[')[1].Split(']')[0];
                    if (int.TryParse(productIdStr, out var productId))
                    {
                        var quantityKey = $"SelectedProducts[{productId}].QuantityToOrder";
                        var unitCostKey = $"SelectedProducts[{productId}].UnitCost";

                        if (int.TryParse(Request.Form[quantityKey], out var quantity) &&
                            decimal.TryParse(Request.Form[unitCostKey], out var unitCost))
                        {
                            selectedProducts.Add(new OrderProductItem
                            {
                                ProductId = productId,
                                Quantity = quantity,
                                UnitPrice = unitCost
                            });
                        }
                    }
                }
            }

            if (!selectedProducts.Any())
            {
                ModelState.AddModelError("", "Please select at least one product");
                model.AvailableSuppliers = await _supplierService.GetAllSuppliersForDropdownAsync();
                return View(model);
            }

            try
            {
                var orderId = await _purchaseOrderService.CreatePurchaseOrderAsync(
                    model.SelectedSupplierId,
                    selectedProducts);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating order: {ex.Message}");
                model.AvailableSuppliers = await _supplierService.GetAllSuppliersForDropdownAsync();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsForSupplier(int supplierId)
        {
            try
            {
                var products = await _purchaseOrderService.GetProductsBySupplierIdAsync(supplierId);

                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsTotal = products.Count,
                    recordsFiltered = products.Count,
                    data = products.Select(p => new ProductSelectionModel
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        ProductSku = p.Sku,
                        CurrentStock = p.StockQuantity,
                        UnitCost = p.Price,
                        QuantityToOrder = 1
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
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
        public virtual async Task<IActionResult> ExportCsv(PurchaseOrderSearchModel searchModel)
        {
            var pagedOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync(searchModel);

            var bytes = _exportManager.ExportPurchaseOrdersToCsv(pagedOrders.ToList());

            return File(bytes, MimeTypes.TextCsv, "purchaseorders.csv");
        }
    }
}
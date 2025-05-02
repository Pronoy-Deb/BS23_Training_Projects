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
using Microsoft.EntityFrameworkCore;
using LinqToDB;

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
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository)
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
                model.AvailableSuppliers = await _suppliersService.GetAllSuppliersForDropdownAsync();
                return View(model);
            }

            // Get supplier details
            //var supplier = await _suppliersService.GetSupplierByIdAsync(model.SelectedSupplierId);
            //if (supplier == null)
            //{
            //    ModelState.AddModelError("", "Selected supplier not found");
            //    model.AvailableSuppliers = await _suppliersService.GetAllSuppliersForDropdownAsync();
            //    return View(model);
            //}

            // Process selected products
            var selectedProducts = new List<ProductSelectionModel>();
            foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("SelectedProducts[")))
            {
                if (key.Contains("].Selected") && Request.Form[key] == "true")
                {
                    var productIdStr = key.Split('[')[1].Split(']')[0];
                    if (int.TryParse(productIdStr, out var productId))
                    {
                        // Get product from database instead of form
                        var product = await _productService.GetProductByIdAsync(productId);
                        if (product == null)
                        {
                            continue; // or handle error as needed
                        }

                        // Get picture URL
                        var picture = (await _productService.GetProductPicturesByProductIdAsync(productId)).FirstOrDefault();
                        var pictureUrl = await _pictureService.GetPictureUrlAsync(picture?.PictureId ?? 0);

                        // Safely parse quantity and unit cost
                        var quantityValid = int.TryParse(Request.Form[$"SelectedProducts[{productId}].QuantityToOrder"], out var quantity);
                        var unitCostValid = decimal.TryParse(Request.Form[$"SelectedProducts[{productId}].UnitCost"], out var unitCost);

                        if (quantityValid && unitCostValid)
                        {
                            selectedProducts.Add(new ProductSelectionModel
                            {
                                ProductId = productId,
                                ProductName = product.Name,
                                ProductSku = product.Sku,
                                PictureThumbnailUrl = pictureUrl,
                                QuantityToOrder = quantity,
                                UnitCost = unitCost,
                                Selected = true
                            });
                        }
                    }
                }
            }

            if (!selectedProducts.Any())
            {
                ModelState.AddModelError("", "Please select at least one product");
                model.AvailableSuppliers = await _suppliersService.GetAllSuppliersForDropdownAsync();
                return View(model);
            }

            try
            {
                // Create and save order
                var order = new PurchaseOrderRecord
                {
                    OrderDate = DateTime.UtcNow,
                    SupplierId = model.SelectedSupplierId,
                    SupplierName = model.SelectedSupplierName,
                    TotalAmount = selectedProducts.Sum(p => p.TotalCost),
                };

                await _purchaseOrderRepository.InsertAsync(order);

                // Save products
                foreach (var product in selectedProducts)
                {
                    await _purchaseOrderProductRepository.InsertAsync(new PurchaseOrderProductRecord
                    {
                        PurchaseOrderId = order.Id,
                        ProductId = product.ProductId,
                        Quantity = product.QuantityToOrder,
                        UnitPrice = product.UnitCost,
                        ProductName = product.ProductName,
                        ProductSku = product.ProductSku,
                        PictureThumbnailUrl = product.PictureThumbnailUrl
                    });
                }

                return Json(new
                {
                    success = true,
                    redirect = Url.Action("List"),
                    orderId = order.Id
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


        //[HttpPost]
        //public async Task<IActionResult> Create(PurchaseOrderCreateModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.AvailableSuppliers = await _suppliersService.GetAllSuppliersForDropdownAsync();
        //        return View(model);
        //    }

        //    // Get selected products from form data
        //    var selectedProducts = new List<OrderProductItem>();
        //    foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("SelectedProducts[")))
        //    {
        //        if (key.Contains("].Selected") && Request.Form[key] == "true")
        //        {
        //            var productIdStr = key.Split('[')[1].Split(']')[0];
        //            if (int.TryParse(productIdStr, out var productId))
        //            {
        //                var quantityKey = $"SelectedProducts[{productId}].QuantityToOrder";
        //                var unitCostKey = $"SelectedProducts[{productId}].UnitCost";

        //                if (int.TryParse(Request.Form[quantityKey], out var quantity) &&
        //                    decimal.TryParse(Request.Form[unitCostKey], out var unitCost))
        //                {
        //                    selectedProducts.Add(new OrderProductItem
        //                    {
        //                        ProductId = productId,
        //                        Quantity = quantity,
        //                        UnitPrice = unitCost
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    if (!selectedProducts.Any())
        //    {
        //        ModelState.AddModelError("", "Please select at least one product");
        //        model.AvailableSuppliers = await _suppliersService.GetAllSuppliersForDropdownAsync();
        //        return View(model);
        //    }

        //    try
        //    {
        //        // Create the purchase order
        //        var order = new PurchaseOrderRecord
        //        {
        //            OrderDate = DateTime.UtcNow,
        //            SupplierId = model.SelectedSupplierId,
        //            SupplierName = model.SelectedSupplierName,
        //            TotalAmount = selectedProducts.Sum(p => p.Quantity * p.UnitPrice)
        //        };

        //        // Insert the order
        //        await _purchaseOrderRepository.InsertAsync(order);

        //        // Add order products
        //        foreach (var product in selectedProducts)
        //        {
        //            await _purchaseOrderProductRepository.InsertAsync(new PurchaseOrderProductRecord
        //            {
        //                PurchaseOrderId = order.Id,
        //                ProductId = product.ProductId,
        //                Quantity = product.Quantity,
        //                UnitPrice = product.UnitPrice
        //            });
        //        }

        //        return Json(new { success = true, redirect = Url.Action("List") });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetProductsForSupplier(int supplierId)
        //{
        //    try
        //    {
        //        var products = await _purchaseOrderService.GetProductsBySupplierIdAsync(supplierId);

        //        return Json(new
        //        {
        //            draw = Request.Form["draw"].FirstOrDefault(),
        //            recordsTotal = products.Count,
        //            recordsFiltered = products.Count,
        //            data = products.Select(p => new ProductSelectionModel
        //            {
        //                ProductId = p.Id,
        //                ProductName = p.Name,
        //                ProductSku = p.Sku,
        //                CurrentStock = p.StockQuantity,
        //                UnitCost = p.Price,
        //                QuantityToOrder = 1
        //            })
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}

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
            //if (string.IsNullOrEmpty(supplierName))
            //{
            //    var supplier = await _suppliersService.GetSupplierByIdAsync(order.SupplierId);
            //    supplierName = supplier?.Name ?? "Supplier Not Found";
            //}

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

        //public async Task<IActionResult> ViewSnapshot(int id)
        //{
        //var order = await LinqToDB.AsyncExtensions.FirstOrDefaultAsync(
        //    _purchaseOrderRepository.Table.Where(o => o.Id == id)
        //);

        //    if (order == null)
        //        return NotFound();

        //    var model = new PurchaseOrderSnapshotModel
        //    {
        //        Id = order.Id,
        //        OrderDate = order.OrderDate,
        //        SupplierName = order.SupplierName,
        //        TotalAmount = order.TotalAmount,
        //        Products = order.Products.Select(p => new PurchaseOrderSnapshotModel.OrderProductSnapshot
        //        {
        //            ProductName = p.ProductName,
        //            ProductSku = p.ProductSku,
        //            PictureThumbnailUrl = p.PictureThumbnailUrl,
        //            Quantity = p.Quantity,
        //            UnitPrice = p.UnitPrice
        //        }).ToList()
        //    };

        //    return View(model);
        //}

        [HttpPost]
        public virtual async Task<IActionResult> ExportCsv(PurchaseOrderSearchModel searchModel)
        {
            var pagedOrders = await _purchaseOrderService.GetAllPurchaseOrdersAsync(searchModel);

            var bytes = _exportManager.ExportPurchaseOrdersToCsv(pagedOrders.ToList());

            return File(bytes, MimeTypes.TextCsv, "purchaseorders.csv");
        }
    }
}
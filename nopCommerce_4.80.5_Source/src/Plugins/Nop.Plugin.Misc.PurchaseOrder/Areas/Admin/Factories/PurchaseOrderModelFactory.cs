using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;
using Nop.Services.Catalog;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Services;
using Nop.Core.Domain.Directory;
using Nop.Services.Directory;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories
{
    public class PurchaseOrderModelFactory : IPurchaseOrderModelFactory
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ISuppliersService _supplierService;
        private readonly ILocalizationService _localizationService;

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly IProductService _productService;

        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IPictureService _pictureService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IUrlRecordService _urlRecordService;

        public PurchaseOrderModelFactory(
            IPurchaseOrderService purchaseOrderService,
            ISuppliersService supplierService,
            ILocalizationService localizationService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            IWorkContext workContext,
            IProductService productService,
            ICurrencyService currencyService,
            CurrencySettings currencySettings,
            IPictureService pictureService,
            IPriceFormatter priceFormatter,
            IUrlRecordService urlRecordService)
        {
            _purchaseOrderService = purchaseOrderService;
            _supplierService = supplierService;
            _localizationService = localizationService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _storeService = storeService;
            _vendorService = vendorService;
            _workContext = workContext;
            _productService = productService;
            _currencyService = currencyService;
            _pictureService = pictureService;
            _currencySettings = currencySettings;
            _priceFormatter = priceFormatter;
            _urlRecordService = urlRecordService;
        }

        public async Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrder = await _purchaseOrderService.GetAllPurchaseOrdersAsync(searchModel);

            var model = new PurchaseOrderListModel().PrepareToGrid(searchModel, purchaseOrder, () =>
            {
                return purchaseOrder;
            });

            return model;
        }

        public async Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model, PurchaseOrderRecord order)
        {
            if (order != null)
            {
                model ??= new PurchaseOrderModel
                {
                    Id = order.Id,
                    SupplierId = order.SupplierId,
                    //Quantity = order.Quantity,
                    Price = order.TotalAmount,
                    OrderDate = DateTime.UtcNow,
                    //OrderStatus = order.OrderStatus
                };
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SupplierId
            }).ToList();

            return model;
        }

        public async Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelAsync(PurchaseOrderCreateModel model, PurchaseOrderRecord order)
        {
            if (order != null)
            {
                model ??= new PurchaseOrderCreateModel
                {
                    OrderDate = DateTime.UtcNow,
                    SupplierId = order.SupplierId,
                    OrderTotal = order.TotalAmount,
                };
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SupplierId
            }).ToList();

            model.AvailableSuppliers.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });

            return model;
        }

        public async Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelForSupplierAsync(PurchaseOrderCreateModel model, int? supplierId)
        {
            model ??= new PurchaseOrderCreateModel();

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = supplierId.HasValue && s.Id == supplierId.Value
            }).ToList();

            // Set default page size
            model.PageSize = 10;
            model.AvailablePageSizes = "10, 25, 50, 100, 500";

            // If a supplier is selected, load their products
            if (supplierId.HasValue && supplierId > 0)
            {
                var products = await _purchaseOrderService.GetProductsBySupplierIdAsync(supplierId.Value);
                model.SelectedProducts = products.Select(p => new ProductSelectionModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    ProductSku = p.Sku,
                    CurrentStock = p.StockQuantity,
                    UnitCost = p.Price,
                    QuantityToOrder = 1
                }).ToList();
            }

            return model;
        }

        public virtual async Task<AddProductToPurchaseOrderSearchModel> PrepareAddProductToPurchaseOrderSearchModelAsync(AddProductToPurchaseOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            // Prepare available categories
            var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);
            //searchModel.AvailableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All")});
            searchModel.AvailableCategories.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
            foreach (var category in categories)
                searchModel.AvailableCategories.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });

            // Prepare available manufacturers
            var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: true);
            searchModel.AvailableManufacturers.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
            //searchModel.AvailableManufacturers.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var manufacturer in manufacturers)
                searchModel.AvailableManufacturers.Add(new SelectListItem { Text = manufacturer.Name, Value = manufacturer.Id.ToString() });

            // Prepare available stores
            var stores = await _storeService.GetAllStoresAsync();
            searchModel.AvailableStores.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
            //searchModel.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });
            foreach (var store in stores)
                searchModel.AvailableStores.Add(new SelectListItem { Text = store.Name, Value = store.Id.ToString() });

            // Prepare available vendors
            var vendors = await _vendorService.GetAllVendorsAsync(showHidden: true);
            searchModel.AvailableVendors.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var vendor in vendors)
                searchModel.AvailableVendors.Add(new SelectListItem { Text = vendor.Name, Value = vendor.Id.ToString() });

            // Prepare available product types
            searchModel.AvailableProductTypes.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.All"), Value = "0" });
            foreach (var productType in Enum.GetValues(typeof(ProductType)).Cast<ProductType>())
            {
                var localizedName = await _localizationService.GetLocalizedEnumAsync(productType);
                searchModel.AvailableProductTypes.Add(new SelectListItem
                {
                    Text = localizedName,
                    Value = ((int)productType).ToString()
                });
            }

            // Set grid page size
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual async Task<ProductListModel> PrepareProductListModelAsync(AddProductToPurchaseOrderSearchModel searchModel)
        {
            ArgumentNullException.ThrowIfNull(searchModel);

            //get parameters to filter comments
            var overridePublished = searchModel.SearchPublishedId == 0 ? null : (bool?)(searchModel.SearchPublishedId == 1);
            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null)
                searchModel.SearchVendorId = currentVendor.Id;
            var categoryIds = new List<int> { searchModel.SearchCategoryId };
            //if (searchModel.SearchIncludeSubCategories && searchModel.SearchCategoryId > 0)
            //{
            //    var childCategoryIds = await _categoryService.GetChildCategoryIdsAsync(parentCategoryId: searchModel.SearchCategoryId, showHidden: true);
            //    categoryIds.AddRange(childCategoryIds);
            //}

            //get products
            var products = await _productService.SearchProductsAsync(showHidden: true,
                categoryIds: categoryIds,
                manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
                storeId: searchModel.SearchStoreId,
                vendorId: searchModel.SearchVendorId,
                //warehouseId: searchModel.SearchWarehouseId,
                productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
            overridePublished: overridePublished);

            var primaryStoreCurrency = await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId);

            //prepare list model
            var model = await new ProductListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async product =>
                {
                    //fill in model values from the entity
                    var productModel = product.ToModel<ProductModel>();

                    //little performance optimization: ensure that "FullDescription" is not returned
                    productModel.FullDescription = string.Empty;

                    //fill formatted price
                    productModel.FormattedPrice = product.ProductType == ProductType.GroupedProduct ? null : await _priceFormatter.FormatPriceAsync(product.Price);

                    productModel.PrimaryStoreCurrencyCode = primaryStoreCurrency.CurrencyCode;

                    //fill in additional values (not existing in the entity)
                    productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);
                    var defaultProductPicture = (await _pictureService.GetPicturesByProductIdAsync(product.Id, 1)).FirstOrDefault();
                    (productModel.PictureThumbnailUrl, _) = await _pictureService.GetPictureUrlAsync(defaultProductPicture, 75);
                    productModel.ProductTypeName = await _localizationService.GetLocalizedEnumAsync(product.ProductType);
                    if (product.ProductType == ProductType.SimpleProduct && product.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
                        productModel.StockQuantityStr = (await _productService.GetTotalStockQuantityAsync(product)).ToString();

                    return productModel;
                });
            });

            return model;
        }

        public async Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            searchModel.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            searchModel.AvailableSuppliers.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public async Task InsertPurchaseOrderAsync(PurchaseOrderModel model)
        {
            var entity = new PurchaseOrderRecord
            {
                SupplierId = model.SupplierId,
                //Quantity = model.Quantity,
                TotalAmount = model.Price,
                OrderDate = DateTime.UtcNow,
                //OrderStatus = model.OrderStatus
            };

            await _purchaseOrderService.InsertPurchaseOrderAsync(entity);
        }

        public async Task<PurchaseOrderModel> GetPurchaseOrderByIdAsync(int id)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (entity == null)
                return null;

            var model = new PurchaseOrderModel
            {
                Id = entity.Id,
                SupplierId = entity.SupplierId,
                //Quantity = entity.Quantity,
                Price = entity.TotalAmount,
                OrderDate = DateTime.UtcNow,
                //OrderStatus = entity.OrderStatus
            };

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SupplierId
            }).ToList();

            return model;
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrderModel model)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(model.Id);
            if (entity == null)
                throw new Exception("Purchase order not found");

            entity.SupplierId = model.SupplierId;
            //entity.Quantity = model.Quantity;
            entity.TotalAmount = model.Price;
            entity.OrderDate = DateTime.UtcNow;
            //entity.OrderStatus = model.OrderStatus;

            await _purchaseOrderService.UpdatePurchaseOrderAsync(entity);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (entity == null)
                throw new Exception("Purchase order not found");

            await _purchaseOrderService.DeletePurchaseOrderAsync(entity);
        }
    }
}

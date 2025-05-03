using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;
using Nop.Services.Catalog;
using Nop.Services.Stores;
using Nop.Services.Vendors;

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
        private readonly IProductService _productService;
        public PurchaseOrderModelFactory(
            IPurchaseOrderService purchaseOrderService,
            ISuppliersService supplierService,
            ILocalizationService localizationService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            IProductService productService)
        {
            _purchaseOrderService = purchaseOrderService;
            _supplierService = supplierService;
            _localizationService = localizationService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _storeService = storeService;
            _vendorService = vendorService;
            _productService = productService;
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

        public async Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelAsync(PurchaseOrderCreateModel model, PurchaseOrderRecord order)
        {
            if (order != null)
            {
                model ??= new PurchaseOrderCreateModel
                {
                    OrderDate = DateTime.Now,
                    SelectedSupplierId = order.SupplierId,
                    OrderTotal = order.TotalAmount,
                };
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SelectedSupplierId
            }).ToList();

            model.AvailableSuppliers.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });

            return model;
        }

        public virtual async Task<AddProductToPurchaseOrderSearchModel> PrepareAddProductToPurchaseOrderSearchModelAsync(AddProductToPurchaseOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            // Prepare available categories
            var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);
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
            foreach (var manufacturer in manufacturers)
                searchModel.AvailableManufacturers.Add(new SelectListItem { Text = manufacturer.Name, Value = manufacturer.Id.ToString() });

            var stores = await _storeService.GetAllStoresAsync();
            searchModel.AvailableStores.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
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
    }
}

using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record AddProductToPurchaseOrderSearchModel : BaseSearchModel
    {
        public AddProductToPurchaseOrderSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByProductName")]
        public string SearchProductName { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderPopup.SearchCategoryId")]
        public int SearchCategoryId { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderPopup.SearchManufacturerId")]
        public int SearchManufacturerId { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderPopup.SearchStoreId")]
        public int SearchStoreId { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderPopup.SearchVendorId")]
        public int SearchVendorId { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderPopup.SearchProductTypeId")]
        public int SearchProductTypeId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }
        public IList<SelectListItem> AvailableProductTypes { get; set; }

        public int PurchaseOrderId { get; set; }
    }
}
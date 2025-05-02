using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public string SearchProductName { get; set; }
        public int SearchCategoryId { get; set; }
        public int SearchManufacturerId { get; set; }
        public int SearchStoreId { get; set; }
        public int SearchVendorId { get; set; }
        public int SearchProductTypeId { get; set; }
        public int SearchPublishedId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }
        public IList<SelectListItem> AvailableProductTypes { get; set; }

        public int PurchaseOrderId { get; set; }
    }
}
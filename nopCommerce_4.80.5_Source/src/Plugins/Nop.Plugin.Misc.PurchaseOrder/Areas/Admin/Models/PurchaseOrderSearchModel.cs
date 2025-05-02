using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByStartDate")]
        public DateTime? StartDate { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByEndDate")]
        public DateTime? EndDate { get; set; }
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByProductName")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchBySupplierName")]
        public int? SupplierId { get; set; } = null;
        [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchProductSku")]
        public List<SelectListItem> AvailableSuppliers { get; set; }
    }
}


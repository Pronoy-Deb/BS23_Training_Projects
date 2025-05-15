using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderCreateModel : BaseNopEntityModel
    {
        public DateTime OrderDate { get; set; }
        public int PageSize { get; set; }
        public string AvailablePageSizes { get; set; }
        [NopResourceDisplayName("Admin.PurchaseOrder.Fields.SelectedSupplierId")]
        public int SelectedSupplierId { get; set; }
        public List<SelectListItem> AvailableSuppliers { get; set; } 
        public List<ProductSelectionModel> SelectedProducts { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
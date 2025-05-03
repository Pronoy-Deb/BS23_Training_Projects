using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderCreateModel : BaseNopEntityModel
    {
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int PageSize { get; set; }
        public string AvailablePageSizes { get; set; }
        public int SelectedSupplierId { get; set; }
        public List<SelectListItem> AvailableSuppliers { get; set; } 
        public List<ProductSelectionModel> SelectedProducts { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
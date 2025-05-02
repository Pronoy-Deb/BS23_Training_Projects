using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderCreateModel : BaseNopEntityModel
    {
        public DateTime OrderDate { get; set; }
        //public int SupplierId { get; set; }
        public string OrderStatus { get; set; }
        public int PageSize { get; set; }
        public string AvailablePageSizes { get; set; }
        public int SelectedSupplierId { get; set; }
        public IList<SelectListItem> AvailableSuppliers { get; set; } // List of suppliers for the dropdown
        public List<ProductSelectionModel> SelectedProducts { get; set; } // List of products to be added to the order
        public decimal OrderTotal { get; set; } // Total cost of the order

    }
}
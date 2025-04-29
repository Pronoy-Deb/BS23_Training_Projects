using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderModel : BaseNopEntityModel
    {
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
    }
}
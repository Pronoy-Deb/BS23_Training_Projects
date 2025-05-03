using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record PurchaseOrderModel : BaseNopEntityModel
    {
        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalPrice { get; set; }
        public string CreatedBy { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
    }
}
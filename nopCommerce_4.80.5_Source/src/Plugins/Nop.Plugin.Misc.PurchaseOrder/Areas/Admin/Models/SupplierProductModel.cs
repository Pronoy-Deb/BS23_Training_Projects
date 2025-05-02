using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models
{
    public record SupplierProductModel : ProductModel
    {
        public int Quantity { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }
    }
}
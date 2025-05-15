using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

public record PurchaseOrderSnapshotModel
{
    public int Id { get; set; }
    [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderView.OrderDate")]
    public DateTime OrderDate { get; set; }
    [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderView.SupplierName")]
    public string SupplierName { get; set; }
    [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderView.TotalAmount")]
    public decimal TotalAmount { get; set; }
    [NopResourceDisplayName("Plugins.Misc.PurchaseOrder.PurchaseOrderView.CreatedBy")]
    public string CreatedBy { get; set; }
    public List<OrderProductSnapshot> Products { get; set; } = new();

    public record OrderProductSnapshot
    {
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string PictureThumbnailUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
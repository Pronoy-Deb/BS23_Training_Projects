namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

public record PurchaseOrderSnapshotModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string SupplierName { get; set; }
    public decimal TotalAmount { get; set; }
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
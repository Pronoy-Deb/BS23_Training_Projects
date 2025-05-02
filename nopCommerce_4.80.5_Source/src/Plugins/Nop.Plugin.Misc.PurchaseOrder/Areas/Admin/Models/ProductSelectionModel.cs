namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

public class ProductSelectionModel
{
    public int ProductId { get; set; }
    public string PictureThumbnailUrl { get; set; }
    public string ProductName { get; set; }
    public string ProductSku { get; set; }
    public int CurrentStock { get; set; }
    public int QuantityToOrder { get; set; }
    public decimal UnitCost { get; set; }
    public bool Selected { get; set; }
}

public class OrderProductItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Misc.PurchaseOrder.Domain
{
    public class PurchaseOrderProductRecord : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string PictureThumbnailUrl { get; set; }
        public virtual PurchaseOrderRecord PurchaseOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
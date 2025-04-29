using Nop.Core.Domain.Localization;
using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Domain
{
    public class PurchaseOrderRecord : BaseEntity, ILocalizedEntity
    {
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
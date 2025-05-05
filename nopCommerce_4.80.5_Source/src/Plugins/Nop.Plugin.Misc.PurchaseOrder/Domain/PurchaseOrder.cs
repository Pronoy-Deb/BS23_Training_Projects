using Nop.Core.Domain.Localization;
using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Domain
{
    public class PurchaseOrder : BaseEntity, ILocalizedEntity
    {
        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
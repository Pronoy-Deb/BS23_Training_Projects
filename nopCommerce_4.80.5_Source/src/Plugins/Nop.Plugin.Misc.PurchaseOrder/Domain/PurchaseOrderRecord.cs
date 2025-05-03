using Nop.Core.Domain.Localization;
using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Domain
{
    public class PurchaseOrderRecord : BaseEntity, ILocalizedEntity
    {
        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual SuppliersRecord Supplier { get; set; }
        public virtual ICollection<PurchaseOrderProductRecord> Products { get; set; } = new List<PurchaseOrderProductRecord>();
    }
}
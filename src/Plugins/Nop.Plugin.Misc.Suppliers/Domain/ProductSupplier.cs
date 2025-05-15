using Nop.Core;

namespace Nop.Plugin.Misc.Suppliers.Domain;
public class ProductSupplier : BaseEntity
{
    public int ProductId { get; set; }

    public int SupplierId { get; set; }
}
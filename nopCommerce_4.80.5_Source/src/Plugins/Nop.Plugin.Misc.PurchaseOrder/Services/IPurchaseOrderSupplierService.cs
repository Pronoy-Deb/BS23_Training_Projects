using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public interface IPurchaseOrderSupplierService
    {
        Task<List<SuppliersRecord>> GetAllSuppliersAsync();
    }
}
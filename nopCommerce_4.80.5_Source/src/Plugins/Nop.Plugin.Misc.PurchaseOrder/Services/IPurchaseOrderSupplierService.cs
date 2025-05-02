using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public interface IPurchaseOrderSupplierService
    {
        Task<List<SuppliersRecord>> GetAllSuppliersAsync();

        Task<IPagedList<Product>> SearchProductsBySupplierAsync(
            int supplierId,
            IList<Product> products,
            string keywords,
            int pageIndex,
            int pageSize);
    }
}
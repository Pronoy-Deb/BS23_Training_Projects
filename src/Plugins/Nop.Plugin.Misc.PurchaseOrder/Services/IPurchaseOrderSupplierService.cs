using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public interface IPurchaseOrderSupplierService
    {
        Task<IPagedList<Product>> SearchProductsBySupplierAsync(
            int supplierId,
            IList<Product> products,
            string keywords,
            int pageIndex,
            int pageSize);
    }
}
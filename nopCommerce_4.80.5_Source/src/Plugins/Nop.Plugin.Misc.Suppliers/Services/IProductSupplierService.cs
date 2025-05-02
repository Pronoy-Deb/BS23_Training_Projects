using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Misc.Suppliers.Services;
public interface IProductSupplierService
{
    Task UpdateProductSupplierAsync(int productId, int supplierId);
    Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId);
    Task<IList<Product>> GetProductsBySupplierIdAsync(int supplierId);
}
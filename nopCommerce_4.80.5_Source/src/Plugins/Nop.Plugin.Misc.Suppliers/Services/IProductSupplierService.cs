using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public interface IProductSupplierService
{
    Task UpdateProductSupplierAsync(int productId, int supplierId);
    Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId);
}
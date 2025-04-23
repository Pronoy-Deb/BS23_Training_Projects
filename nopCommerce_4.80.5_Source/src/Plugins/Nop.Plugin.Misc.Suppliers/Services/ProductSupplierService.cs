using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public class ProductSupplierService : IProductSupplierService
{
    private readonly IRepository<ProductSupplier> _productSupplierRepository;

    public ProductSupplierService(IRepository<ProductSupplier> productSupplierRepository)
    {
        _productSupplierRepository = productSupplierRepository;
    }

    public async Task UpdateProductSupplierAsync(int productId, int supplierId)
    {

        if (supplierId > 0)
        {
            await _productSupplierRepository.InsertAsync(new ProductSupplier
            {
                ProductId = productId,
                SupplierId = supplierId
            });
        }
    }

    public async Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId)
    {
        return await _productSupplierRepository.Table
            .Where(ps => ps.ProductId == productId)
            .Select(ps => ps.SupplierId)
            .ToListAsync();
    }
}
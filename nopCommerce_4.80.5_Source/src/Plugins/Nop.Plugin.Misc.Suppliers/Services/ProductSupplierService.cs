using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public Task DeleteProductSupplierAsync(ProductSupplier productSupplier)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ProductSupplier>> GetProductSuppliersByProductIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ProductSupplier>> GetProductSuppliersBySupplierIdAsync(int supplierId)
    {
        throw new NotImplementedException();
    }

    public async Task InsertProductSupplierAsync(ProductSupplier productSupplier)
    {
        await _productSupplierRepository.InsertAsync(productSupplier);
    }

    public Task UpdateProductSupplierAsync(ProductSupplier productSupplier)
    {
        throw new NotImplementedException();
    }
}
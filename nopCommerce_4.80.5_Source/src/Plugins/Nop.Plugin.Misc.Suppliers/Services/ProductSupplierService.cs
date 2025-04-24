using Nop.Data;
using Nop.Core.Caching;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public class ProductSupplierService : IProductSupplierService
{
    private readonly IRepository<ProductSupplier> _productSupplierRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    public ProductSupplierService(IRepository<ProductSupplier> productSupplierRepository, IStaticCacheManager staticCacheManager)
    {
        _productSupplierRepository = productSupplierRepository;
        _staticCacheManager = staticCacheManager;
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
            await _staticCacheManager.RemoveByPrefixAsync(string.Format(SuppliersDefaults.ProductSuppliersPrefixCacheKey, productId));
            //await _staticCacheManager.RemoveByPrefixAsync(SuppliersDefaults.AdminSupplierAllPrefixCacheKey);
            //await _staticCacheManager.RemoveByPrefixAsync(string.Format(SuppliersDefaults.ProductSuppliersPrefixCacheKey, productId));
        }
    }

    public async Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(SuppliersDefaults.ProductSuppliersByProductIdCacheKey, productId);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _productSupplierRepository.Table
                .Where(ps => ps.ProductId == productId)
                .Select(ps => ps.SupplierId)
                .ToListAsync();
        });
    }


    //public async Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId)
    //{
    //    var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
    //        SuppliersDefaults.ProductSuppliersByProductIdCacheKey, productId);

    //    return await _staticCacheManager.GetAsync(cacheKey, async () =>
    //    {
    //        return await _productSupplierRepository.Table
    //            .Where(ps => ps.ProductId == productId)
    //            .Select(ps => ps.SupplierId)
    //            .ToListAsync();
    //    });
    //}

    //public async Task<IList<int>> GetSupplierIdsByProductIdAsync(int productId)
    //{
    //    return await _productSupplierRepository.Table
    //        .Where(ps => ps.ProductId == productId)
    //        .Select(ps => ps.SupplierId)
    //        .ToListAsync();
    //}
}
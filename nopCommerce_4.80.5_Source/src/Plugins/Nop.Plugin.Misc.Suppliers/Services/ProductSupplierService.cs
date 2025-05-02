using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Core.Caching;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services
{
    public class ProductSupplierService : IProductSupplierService
    {
        private readonly IRepository<ProductSupplier> _productSupplierRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        public ProductSupplierService(
            IRepository<ProductSupplier> productSupplierRepository,
            IRepository<Product> productRepository,
            IStaticCacheManager staticCacheManager)
        {
            _productSupplierRepository = productSupplierRepository;
            _productRepository = productRepository;
            _staticCacheManager = staticCacheManager;
        }

        public async Task UpdateProductSupplierAsync(int productId, int supplierId)
        {
            if (supplierId > 0)
            {
                var existingMapping = await _productSupplierRepository.Table
                    .FirstOrDefaultAsync(ps => ps.ProductId == productId);

                if (existingMapping != null)
                    await _productSupplierRepository.DeleteAsync(existingMapping);

                await _productSupplierRepository.InsertAsync(new ProductSupplier
                {
                    ProductId = productId,
                    SupplierId = supplierId
                });

                await _staticCacheManager.RemoveByPrefixAsync(string.Format(SuppliersDefaults.ProductSuppliersPrefixCacheKey, productId));
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

        /// <summary>
        /// Gets all products associated with a supplier.
        /// </summary>
        public async Task<IList<Product>> GetProductsBySupplierIdAsync(int supplierId)
        {
            var productIds = await _productSupplierRepository.Table
                .Where(x => x.SupplierId == supplierId)
                .Select(x => x.ProductId)
                .ToListAsync();

            if (!productIds.Any())
                return new List<Product>();

            return await _productRepository.Table
                .Where(p => productIds.Contains(p.Id) && !p.Deleted)
                .ToListAsync();
        }
    }
}

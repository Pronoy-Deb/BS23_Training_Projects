using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public partial class PurchaseOrderSupplierService : IPurchaseOrderSupplierService
    {
        private readonly IRepository<SuppliersRecord> _supplierRepository;
        private readonly IRepository<ProductSupplier> _productSupplierRepository;
        private readonly IRepository<Product> _productRepository;


        public PurchaseOrderSupplierService(IRepository<SuppliersRecord> supplierRepository,
            IRepository<ProductSupplier> productSupplierRepository,
            IRepository<Product> productRepository)
        {
            _supplierRepository = supplierRepository;
            _productSupplierRepository = productSupplierRepository;
            _productRepository = productRepository;
        }
        public async Task<List<SuppliersRecord>> GetAllSuppliersAsync()
        {
            var suppliers = _supplierRepository.Table.ToList();
            return suppliers;
        }
        public virtual async Task<IPagedList<Product>> SearchProductsBySupplierAsync(
            int supplierId,
            IList<Product> products,
            string keywords,
            int pageIndex,
            int pageSize)
        {
            if (supplierId == 0)
                throw new ArgumentException("Supplier ID must be provided.", nameof(supplierId));

            // Step 1: Get all ProductIds linked to the Supplier
            var supplierProductIds = await _productSupplierRepository.Table
                .Where(ps => ps.SupplierId == supplierId)
                .Select(ps => ps.ProductId)
                .ToListAsync();

            // Step 2: Apply optional product filter if given
            if (products != null && products.Any())
            {
                var inputProductIds = products.Select(p => p.Id).ToList();
                supplierProductIds = supplierProductIds.Intersect(inputProductIds).ToList();
            }

            // Step 3: Query the Product table for matching products
            var productsQuery = _productRepository.Table
                .Where(p => supplierProductIds.Contains(p.Id) && !p.Deleted);

            // Step 4: Apply keyword filtering
            if (!string.IsNullOrWhiteSpace(keywords))
                productsQuery = productsQuery.Where(p => p.Name.Contains(keywords));

            // Step 5: Paginate
            var result = await productsQuery
                .OrderBy(p => p.Name)
                .ToPagedListAsync(pageIndex, pageSize);

            return result;
        }
    }
}
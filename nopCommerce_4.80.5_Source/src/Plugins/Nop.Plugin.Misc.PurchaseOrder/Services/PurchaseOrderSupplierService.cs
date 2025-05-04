using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public partial class PurchaseOrderSupplierService : IPurchaseOrderSupplierService
    {
        private readonly IRepository<ProductSupplier> _productSupplierRepository;
        private readonly IRepository<Product> _productRepository;


        public PurchaseOrderSupplierService(IRepository<ProductSupplier> productSupplierRepository,
            IRepository<Product> productRepository)
        {
            _productSupplierRepository = productSupplierRepository;
            _productRepository = productRepository;
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

            // Get product IDs assigned to the supplier
            var supplierProductIds = await _productSupplierRepository.Table
                .Where(ps => ps.SupplierId == supplierId)
                .Select(ps => ps.ProductId)
                .ToListAsync();

            // Filter by provided products if any
            if (products != null && products.Any())
            {
                var inputProductIds = products.Select(p => p.Id).ToList();
                supplierProductIds = supplierProductIds.Intersect(inputProductIds).ToList();
            }

            // Build the base query
            var productsQuery = _productRepository.Table
                .Where(p => supplierProductIds.Contains(p.Id) && !p.Deleted);

            if (!string.IsNullOrWhiteSpace(keywords))
                productsQuery = productsQuery.Where(p => p.Name.Contains(keywords));

            var result = await productsQuery
                .OrderBy(p => p.Name)
                .ToPagedListAsync(pageIndex, pageSize);

            return result;
        }
    }
}
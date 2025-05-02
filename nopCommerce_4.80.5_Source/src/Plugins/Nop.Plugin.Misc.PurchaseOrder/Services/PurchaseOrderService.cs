using Nop.Data;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrderRecord> _purchaseOrderRepository;
        private readonly IRepository<SuppliersRecord> _suppliersRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<PurchaseOrderProductRecord> _purchaseOrderProductRepository;
        private readonly IRepository<ProductSupplier> _supplierProductRepository;

        public PurchaseOrderService(IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IRepository<SuppliersRecord> suppliersRepository,
            IRepository<Product> productRepository,
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository,
            IRepository<ProductSupplier> supplierProductRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _suppliersRepository = suppliersRepository;
            _productRepository = productRepository;
            _purchaseOrderProductRepository = purchaseOrderProductRepository;
            _supplierProductRepository = supplierProductRepository;
        }

        public async Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrderQuery = _purchaseOrderRepository.Table;
            var supplierQuery = _suppliersRepository.Table;
            var productQuery = _productRepository.Table;

            var query = from po in purchaseOrderQuery
                join supplier in supplierQuery on po.SupplierId equals supplier.Id
                //join product in productQuery on po.ProductId equals product.Id
                select new PurchaseOrderModel
                {
                    Id = po.Id,
                    OrderDate = po.OrderDate,
                    //OrderStatus = po.OrderStatus,
                    SupplierId = po.SupplierId,
                    //Quantity = po.Quantity,
                    Price = po.TotalAmount,
                    CreatedBy = "Admin",
                    SupplierName = supplier.Name,
                    //ProductName = product.Name
                };

            if (searchModel != null)
            {
                if (searchModel.StartDate.HasValue)
                    query = query.Where(x => x.OrderDate >= searchModel.StartDate.Value);

                if (searchModel.EndDate.HasValue)
                    query = query.Where(x => x.OrderDate <= searchModel.EndDate.Value);

                if (searchModel.SupplierId.HasValue && searchModel.SupplierId.Value > 0)
                    query = query.Where(x => x.SupplierId == searchModel.SupplierId.Value);

                if (!string.IsNullOrEmpty(searchModel.ProductName))
                    query = query.Where(x => x.ProductName.Contains(searchModel.ProductName));
            }

            return await query.ToPagedListAsync(searchModel.Page - 1, searchModel.PageSize);
        }

        public async Task<PurchaseOrderRecord> GetPurchaseOrderByIdAsync(int id)
        {
            return await _purchaseOrderRepository.GetByIdAsync(id);
        }

        public async Task InsertPurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.InsertAsync(order);
        }
        public async Task InsertPurchaseOrderProductAsync(PurchaseOrderProductRecord product)
        {
            await _purchaseOrderProductRepository.InsertAsync(product);
        }


        public async Task UpdatePurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.UpdateAsync(order);
        }

        public async Task DeletePurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.DeleteAsync(order);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            if (productId <= 0)
                return null;

            // Fetch the product from the repository by ID
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task<IList<Product>> GetProductsBySupplierIdAsync(int supplierId)
        {
            if (supplierId <= 0)
                return new List<Product>();

            // Get product IDs for this supplier from the junction table
            var productIds = await _supplierProductRepository.Table
                .Where(ps => ps.SupplierId == supplierId)
                .Select(ps => ps.ProductId)
                .Distinct()
                .ToListAsync();

            if (!productIds.Any())
                return new List<Product>();

            // Get full product details
            var products = await _productRepository.Table
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Sku = p.Sku,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    // Include other product fields you need
                    ShortDescription = p.ShortDescription,
                    Published = p.Published
                })
                .ToListAsync();

            return products;
        }

        public async Task<IList<PurchaseOrderProductRecord>> GetProductsByOrderIdsAsync(IList<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
                return new List<PurchaseOrderProductRecord>();

            return await _purchaseOrderProductRepository.Table
                .Where(p => orderIds.Contains(p.PurchaseOrderId))
                .ToListAsync();
        }

        public async Task<int> CreatePurchaseOrderAsync(int supplierId, List<OrderProductItem> products)
        {
            var order = new PurchaseOrderRecord
            {
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                SupplierId = supplierId,
                //OrderStatus = "Pending",
                TotalAmount = products.Sum(p => p.Quantity * p.UnitPrice)
            };

            // Insert the main order first
            await _purchaseOrderRepository.InsertAsync(order);

            // Then add products
            foreach (var product in products)
            {
                var orderProduct = new PurchaseOrderProductRecord
                {
                    PurchaseOrderId = order.Id,
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    UnitPrice = product.UnitPrice
                };
                await _purchaseOrderProductRepository.InsertAsync(orderProduct);
            }

            return order.Id;
        }

        private string GenerateOrderNumber()
        {
            return $"PO-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }
    }
}
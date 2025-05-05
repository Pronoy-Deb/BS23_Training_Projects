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

        public PurchaseOrderService(IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IRepository<SuppliersRecord> suppliersRepository,
            IRepository<Product> productRepository,
            IRepository<PurchaseOrderProductRecord> purchaseOrderProductRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _suppliersRepository = suppliersRepository;
            _productRepository = productRepository;
            _purchaseOrderProductRepository = purchaseOrderProductRepository;
        }

        public async Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(PurchaseOrderSearchModel searchModel)
        {
            var baseQuery = _purchaseOrderRepository.Table;

            if (!string.IsNullOrEmpty(searchModel.ProductName))
            {
                var orderIdsWithProduct = _purchaseOrderProductRepository.Table
                    .Where(p => p.ProductName.Contains(searchModel.ProductName))
                    .Select(p => p.PurchaseOrderId)
                    .Distinct();

                baseQuery = baseQuery.Where(po => orderIdsWithProduct.Contains(po.Id));
            }

            if (searchModel.StartDate.HasValue)
                baseQuery = baseQuery.Where(x => x.OrderDate >= searchModel.StartDate.Value);

            if (searchModel.EndDate.HasValue)
                baseQuery = baseQuery.Where(x => x.OrderDate <= searchModel.EndDate.Value);

            if (searchModel.SupplierId.HasValue && searchModel.SupplierId.Value > 0)
                baseQuery = baseQuery.Where(x => x.SupplierId == searchModel.SupplierId.Value);

            var query = from po in baseQuery
                join supplier in _suppliersRepository.Table on po.SupplierId equals supplier.Id
                select new PurchaseOrderModel
                {
                    Id = po.Id,
                    OrderDate = po.OrderDate,
                    SupplierId = po.SupplierId,
                    TotalPrice = po.TotalAmount,
                    CreatedBy = "Admin",
                    SupplierName = supplier.Name
                };

            query = query.OrderByDescending(po => po.OrderDate);

            return await query.ToPagedListAsync(searchModel.Page - 1, searchModel.PageSize);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            if (productId <= 0)
                return null;

            return await _productRepository.GetByIdAsync(productId);
        }
    }
}
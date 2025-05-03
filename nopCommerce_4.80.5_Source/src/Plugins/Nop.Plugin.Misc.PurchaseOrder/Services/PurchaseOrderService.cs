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

        public PurchaseOrderService(IRepository<PurchaseOrderRecord> purchaseOrderRepository,
            IRepository<SuppliersRecord> suppliersRepository,
            IRepository<Product> productRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _suppliersRepository = suppliersRepository;
            _productRepository = productRepository;
        }

        public async Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrderQuery = _purchaseOrderRepository.Table;
            var supplierQuery = _suppliersRepository.Table;
            var productQuery = _productRepository.Table;

            var query = from po in purchaseOrderQuery
                join supplier in supplierQuery on po.SupplierId equals supplier.Id
                select new PurchaseOrderModel
                {
                    Id = po.Id,
                    OrderDate = po.OrderDate,
                    SupplierId = po.SupplierId,
                    TotalPrice = po.TotalAmount,
                    CreatedBy = "Admin",
                    SupplierName = supplier.Name,
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
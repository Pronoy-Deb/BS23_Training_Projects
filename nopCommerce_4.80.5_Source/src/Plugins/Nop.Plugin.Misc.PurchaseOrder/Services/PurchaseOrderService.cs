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

        public async Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(
            int pageIndex,
            int pageSize)
        {
            var purchaseOrderQuery = _purchaseOrderRepository.Table;
            var supplierQuery = _suppliersRepository.Table;
            var productQuery = _productRepository.Table;

            var query = from po in purchaseOrderQuery
                join supplier in supplierQuery on po.SupplierId equals supplier.Id
                join product in productQuery on po.ProductId equals product.Id
                select new PurchaseOrderModel
                {
                    Id = po.Id,
                    OrderDate = po.OrderDate,
                    OrderStatus = po.OrderStatus,
                    SupplierId = po.SupplierId,
                    Quantity = po.Quantity,
                    Price = po.Price * po.Quantity,
                    CreatedBy = "Admin",
                    SupplierName = supplier.Name,
                    ProductName = product.Name
                };

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        //public async Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(
        //    DateTime? startDate,
        //    DateTime? endDate,
        //    string? supplierName,
        //    string? productName,
        //    int pageIndex,
        //    int pageSize)
        //{
        //    var purchaseOrderQuery = _purchaseOrderRepository.Table;
        //    var supplierQuery = _suppliersRepository.Table;
        //    var productQuery = _productRepository.Table;

        //    var query = from po in purchaseOrderQuery
        //        join supplier in supplierQuery on po.SupplierId equals supplier.Id
        //        join product in productQuery on po.ProductId equals product.Id
        //        where
        //            (string.IsNullOrEmpty(supplierName) || supplier.Name == supplierName) && // <-- Fix
        //            (string.IsNullOrEmpty(productName) || product.Name.Contains(productName)) && // <-- Fix
        //            (!startDate.HasValue || po.OrderDate >= startDate.Value) &&
        //            (!endDate.HasValue || po.OrderDate <= endDate.Value)
        //        select new PurchaseOrderModel
        //        {
        //            Id = po.Id,
        //            OrderDate = po.OrderDate,
        //            OrderStatus = po.OrderStatus,
        //            SupplierId = po.SupplierId,
        //            Quantity = po.Quantity,
        //            Price = po.Price,
        //            CreatedBy = "Admin",
        //            SupplierName = supplier.Name,
        //            ProductName = product.Name
        //        };

        //    return await query.ToPagedListAsync(pageIndex, pageSize);
        //}


        public async Task<PurchaseOrderRecord> GetPurchaseOrderByIdAsync(int id)
        {
            return await _purchaseOrderRepository.GetByIdAsync(id);
        }

        public async Task InsertPurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.InsertAsync(order);
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.UpdateAsync(order);
        }

        public async Task DeletePurchaseOrderAsync(PurchaseOrderRecord order)
        {
            await _purchaseOrderRepository.DeleteAsync(order);
        }

        //public IList<PurchaseOrderModel> GetFilteredOrders(PurchaseOrderSearchModel searchModel)
        //{
        //    // TODO: Fetch filtered data from DB according to searchModel

        //    // For now returning dummy data
        //    return new List<PurchaseOrderModel>
        //    {
        //        new PurchaseOrderModel
        //        {
        //            Id = 1,
        //            SupplierName = "Supplier A",
        //            OrderDate = DateTime.UtcNow,
        //            Price = 500,
        //            CreatedBy = "Admin"
        //        }
        //    };
        //}

        public async Task<IPagedList<PurchaseOrderModel>> GetFilteredOrders(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrderQuery = _purchaseOrderRepository.Table;
            var supplierQuery = _suppliersRepository.Table;
            var productQuery = _productRepository.Table;

            var query = from po in purchaseOrderQuery
                join supplier in supplierQuery on po.SupplierId equals supplier.Id
                join product in productQuery on po.ProductId equals product.Id
                where
                    (!searchModel.StartDate.HasValue || po.OrderDate >= searchModel.StartDate.Value) &&
                    (!searchModel.EndDate.HasValue || po.OrderDate <= searchModel.EndDate.Value) &&
                    (string.IsNullOrEmpty(searchModel.SupplierName) || supplier.Name.Contains(searchModel.SupplierName)) &&
                    (string.IsNullOrEmpty(searchModel.ProductName) || product.Name.Contains(searchModel.ProductName))
                select new PurchaseOrderModel
                {
                    Id = po.Id,
                    OrderDate = po.OrderDate,
                    OrderStatus = po.OrderStatus,
                    SupplierId = po.SupplierId,
                    Quantity = po.Quantity,
                    Price = po.Price,
                    CreatedBy = "Admin",
                    SupplierName = supplier.Name,
                    ProductName = product.Name
                };

            return await query.ToPagedListAsync(searchModel.Page - 1, searchModel.PageSize);
        }

    }
}
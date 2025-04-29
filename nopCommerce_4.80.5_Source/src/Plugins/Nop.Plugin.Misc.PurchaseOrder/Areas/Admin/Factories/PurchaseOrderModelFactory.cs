using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories
{
    public class PurchaseOrderModelFactory : IPurchaseOrderModelFactory
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ISuppliersService _supplierService;
        private readonly ILocalizationService _localizationService;

        public PurchaseOrderModelFactory(
            IPurchaseOrderService purchaseOrderService,
            ISuppliersService supplierService,
            ILocalizationService localizationService)
        {
            _purchaseOrderService = purchaseOrderService;
            _supplierService = supplierService;
            _localizationService = localizationService;
        }

        public async Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel)
        {
            var purchaseOrder = await _purchaseOrderService.GetAllPurchaseOrdersAsync(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize
            );

            var model = new PurchaseOrderListModel().PrepareToGrid(searchModel, purchaseOrder, () =>
            {
                return purchaseOrder;
            });

            return model;
        }

        //public async Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel)
        //{
        //    if (searchModel == null)
        //        throw new ArgumentNullException(nameof(searchModel));

        //    var pagedOrders = await _purchaseOrderService.GetFilteredOrders(searchModel); // should return IPagedList<PurchaseOrder>

        //    var model = new PurchaseOrderListModel().PrepareToGrid(searchModel, pagedOrders, () =>
        //    {
        //        return pagedOrders.Select(po => new PurchaseOrderModel
        //        {
        //            Id = po.Id,
        //            OrderDate = po.OrderDate,
        //            OrderStatus = po.OrderStatus,
        //            SupplierId = po.SupplierId,
        //            Quantity = po.Quantity,
        //            Price = po.Price,
        //            CreatedBy = po.CreatedBy,
        //            SupplierName = po.SupplierName,
        //            ProductName = po.ProductName
        //        });
        //    });

        //    return model;
        //}


        public async Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model, PurchaseOrderRecord order)
        {
            if (order != null)
            {
                model ??= new PurchaseOrderModel
                {
                    Id = order.Id,
                    SupplierId = order.SupplierId,
                    Quantity = order.Quantity,
                    Price = order.Price,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus
                };
            }

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SupplierId
            }).ToList();

            return model;
        }
        public async Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            // Load all suppliers
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            searchModel.AvailableSuppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            // Add a default empty option
            searchModel.AvailableSuppliers.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });

            searchModel.SetGridPageSize();

            return searchModel;
        }

        //public async Task<PurchaseOrderListModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel)
        //{
        //    if (searchModel == null)
        //        throw new ArgumentNullException(nameof(searchModel));

        //    // Get filtered and paged orders
        //    var pagedOrders = await _purchaseOrderService.GetFilteredOrders(searchModel); // Should return IPagedList<PurchaseOrder>

        //    var model = new PurchaseOrderListModel().PrepareToGrid(searchModel, pagedOrders, () =>
        //    {
        //        return pagedOrders.Select(po => new PurchaseOrderModel
        //        {
        //            Id = po.Id,
        //            OrderDate = po.OrderDate,
        //            OrderStatus = po.OrderStatus,
        //            SupplierId = po.SupplierId,
        //            Quantity = po.Quantity,
        //            Price = po.Price,
        //            CreatedBy = po.CreatedBy,
        //            SupplierName = po.SupplierName,
        //            ProductName = po.ProductName
        //        });
        //    });

        //    return model;
        //}


        public async Task InsertPurchaseOrderAsync(PurchaseOrderModel model)
        {
            var entity = new PurchaseOrderRecord
            {
                SupplierId = model.SupplierId,
                Quantity = model.Quantity,
                Price = model.Price,
                OrderDate = model.OrderDate,
                OrderStatus = model.OrderStatus
            };

            await _purchaseOrderService.InsertPurchaseOrderAsync(entity);
        }

        public async Task<PurchaseOrderModel> GetPurchaseOrderByIdAsync(int id)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (entity == null)
                return null;

            var model = new PurchaseOrderModel
            {
                Id = entity.Id,
                SupplierId = entity.SupplierId,
                Quantity = entity.Quantity,
                Price = entity.Price,
                OrderDate = entity.OrderDate,
                OrderStatus = entity.OrderStatus
            };

            var suppliers = await _supplierService.GetAllSuppliersAsync();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == model.SupplierId
            }).ToList();

            return model;
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrderModel model)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(model.Id);
            if (entity == null)
                throw new Exception("Purchase order not found");

            entity.SupplierId = model.SupplierId;
            entity.Quantity = model.Quantity;
            entity.Price = model.Price;
            entity.OrderDate = model.OrderDate;
            entity.OrderStatus = model.OrderStatus;

            await _purchaseOrderService.UpdatePurchaseOrderAsync(entity);
        }

        public async Task DeletePurchaseOrderAsync(int id)
        {
            var entity = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (entity == null)
                throw new Exception("Purchase order not found");

            await _purchaseOrderService.DeletePurchaseOrderAsync(entity);
        }
    }
}

using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Services;
public interface IPurchaseOrderService
{
    //Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(
    //    DateTime? startDate,
    //    DateTime? endDate,
    //    string? supplierName,
    //    string? productName,
    //    int pageIndex,
    //    int pageSize);
    Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(
        int pageIndex,
        int pageSize);

    Task<PurchaseOrderRecord> GetPurchaseOrderByIdAsync(int id);

    Task InsertPurchaseOrderAsync(PurchaseOrderRecord order);

    Task UpdatePurchaseOrderAsync(PurchaseOrderRecord order);

    Task DeletePurchaseOrderAsync(PurchaseOrderRecord order);

    Task<IPagedList<PurchaseOrderModel>> GetFilteredOrders(PurchaseOrderSearchModel searchModel);
    //IList<PurchaseOrderModel> GetFilteredOrders(PurchaseOrderSearchModel searchModel);
}
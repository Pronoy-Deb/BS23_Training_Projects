using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

public interface IPurchaseOrderModelFactory
{
    Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);

    Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
    //Task<PurchaseOrderListModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
    Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model, PurchaseOrderRecord purchaseOrder);
    Task InsertPurchaseOrderAsync(PurchaseOrderModel model);
    Task<PurchaseOrderModel> GetPurchaseOrderByIdAsync(int id);
    Task UpdatePurchaseOrderAsync(PurchaseOrderModel model);
    Task DeletePurchaseOrderAsync(int id);
}
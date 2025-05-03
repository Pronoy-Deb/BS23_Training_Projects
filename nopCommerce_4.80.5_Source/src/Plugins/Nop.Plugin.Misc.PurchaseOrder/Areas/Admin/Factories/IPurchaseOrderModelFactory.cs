using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

public interface IPurchaseOrderModelFactory
{
    Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);
    Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);

    Task<AddProductToPurchaseOrderSearchModel> PrepareAddProductToPurchaseOrderSearchModelAsync(
        AddProductToPurchaseOrderSearchModel searchModel);

    Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelAsync(PurchaseOrderCreateModel model,
        PurchaseOrderRecord order);
}
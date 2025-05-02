using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Web.Areas.Admin.Models.Catalog;

public interface IPurchaseOrderModelFactory
{
    Task<PurchaseOrderListModel> PreparePurchaseOrderListModelAsync(PurchaseOrderSearchModel searchModel);
    Task<PurchaseOrderSearchModel> PreparePurchaseOrderSearchModelAsync(PurchaseOrderSearchModel searchModel);
    Task<PurchaseOrderModel> PreparePurchaseOrderModelAsync(PurchaseOrderModel model, PurchaseOrderRecord purchaseOrder);

    Task<AddProductToPurchaseOrderSearchModel> PrepareAddProductToPurchaseOrderSearchModelAsync(
        AddProductToPurchaseOrderSearchModel searchModel);

    Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelAsync(PurchaseOrderCreateModel model,
        PurchaseOrderRecord order);

    Task<ProductListModel> PrepareProductListModelAsync(AddProductToPurchaseOrderSearchModel searchModel);

    Task<PurchaseOrderCreateModel> PreparePurchaseOrderCreateModelForSupplierAsync(PurchaseOrderCreateModel model, int? supplierId);
    Task InsertPurchaseOrderAsync(PurchaseOrderModel model);
    Task<PurchaseOrderModel> GetPurchaseOrderByIdAsync(int id);
    Task UpdatePurchaseOrderAsync(PurchaseOrderModel model);
    Task DeletePurchaseOrderAsync(int id);
}
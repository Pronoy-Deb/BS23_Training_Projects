using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Services;
public interface IPurchaseOrderService
{
    Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(PurchaseOrderSearchModel searchModel);
    Task<PurchaseOrderRecord> GetPurchaseOrderByIdAsync(int id);

    Task InsertPurchaseOrderAsync(PurchaseOrderRecord order);
    Task InsertPurchaseOrderProductAsync(PurchaseOrderProductRecord product);

    Task<IList<PurchaseOrderProductRecord>> GetProductsByOrderIdsAsync(IList<int> orderIds);

    Task<IList<Product>> GetProductsBySupplierIdAsync(int supplierId);
    Task<Product> GetProductByIdAsync(int productId);
    Task UpdatePurchaseOrderAsync(PurchaseOrderRecord order);
    Task<int> CreatePurchaseOrderAsync(int supplierId, List<OrderProductItem> products);

    Task DeletePurchaseOrderAsync(PurchaseOrderRecord order);
}
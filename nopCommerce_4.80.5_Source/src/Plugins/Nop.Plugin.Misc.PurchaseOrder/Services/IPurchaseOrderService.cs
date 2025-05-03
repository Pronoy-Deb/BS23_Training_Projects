using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Services;
public interface IPurchaseOrderService
{
    Task<IPagedList<PurchaseOrderModel>> GetAllPurchaseOrdersAsync(PurchaseOrderSearchModel searchModel);

    Task<Product> GetProductByIdAsync(int productId);
}
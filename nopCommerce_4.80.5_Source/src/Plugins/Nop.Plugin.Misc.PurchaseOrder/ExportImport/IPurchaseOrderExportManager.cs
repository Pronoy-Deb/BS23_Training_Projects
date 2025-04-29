using System.Collections.Generic;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.ExportImport
{
    public interface IPurchaseOrderExportManager
    {
        byte[] ExportPurchaseOrdersToCsv(IList<PurchaseOrderModel> orders);
    }
}
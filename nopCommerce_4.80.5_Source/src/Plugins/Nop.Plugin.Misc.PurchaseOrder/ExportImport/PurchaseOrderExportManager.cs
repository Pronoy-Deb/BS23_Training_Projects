using System.Text;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.ExportImport
{
    public class PurchaseOrderExportManager : IPurchaseOrderExportManager
    {
        public byte[] ExportPurchaseOrdersToCsv(IList<PurchaseOrderModel> orders)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                writer.WriteLine("Order ID, Supplier Name, Order Date, Total Amount, Created By");

                foreach (var order in orders)
                {
                    writer.WriteLine($"{order.Id},{order.SupplierName},{order.OrderDate},{order.TotalPrice},{order.CreatedBy}");
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
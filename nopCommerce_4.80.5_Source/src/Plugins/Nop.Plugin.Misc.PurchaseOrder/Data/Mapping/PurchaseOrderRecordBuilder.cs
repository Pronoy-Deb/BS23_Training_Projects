using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Mapping
{
    public class PurchaseOrderRecordBuilder : NopEntityBuilder<PurchaseOrderRecord>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PurchaseOrderRecord.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PurchaseOrderRecord.OrderDate)).AsDateTime().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.SupplierId)).AsInt32().NotNullable()
                .ForeignKey("FK_PurchaseOrder_Supplier", nameof(SuppliersRecord), "Id")
                .WithColumn(nameof(PurchaseOrderRecord.SupplierName)).AsString(400).Nullable()
                .WithColumn(nameof(PurchaseOrderRecord.TotalAmount)).AsDecimal(18, 4).NotNullable();
        }
    }
}
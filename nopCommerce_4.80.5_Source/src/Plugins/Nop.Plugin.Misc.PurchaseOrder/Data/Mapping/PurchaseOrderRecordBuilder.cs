using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Mapping
{
    public class PurchaseOrderRecordBuilder : NopEntityBuilder<PurchaseOrderRecord>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PurchaseOrderRecord.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PurchaseOrderRecord.OrderNumber)).AsString(50).NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.OrderDate)).AsDateTime().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.SupplierId)).AsInt32().NotNullable()
                .ForeignKey("FK_PurchaseOrder_Supplier", "SuppliersRecord", "Id")
                .WithColumn(nameof(PurchaseOrderRecord.TotalAmount)).AsDecimal(18, 4).NotNullable();
        }
    }
}
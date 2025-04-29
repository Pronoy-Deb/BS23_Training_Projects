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
                .WithColumn(nameof(PurchaseOrderRecord.OrderDate)).AsDateTime().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.SupplierId)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.ProductId)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.OrderStatus)).AsString(100).NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.Quantity)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderRecord.Price)).AsDecimal().NotNullable();
        }
    }
}
using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Mapping
{
    public class PurchaseOrderProductRecordBuilder : NopEntityBuilder<PurchaseOrderProductRecord>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PurchaseOrderProductRecord.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PurchaseOrderProductRecord.PurchaseOrderId)).AsInt32().NotNullable()
                .ForeignKey<PurchaseOrderRecord>(onDelete: Rule.Cascade)
                .WithColumn(nameof(PurchaseOrderProductRecord.ProductId)).AsInt32().NotNullable()
                .ForeignKey("FK_PurchaseOrderProduct_Product", "Product", "Id")
                .WithColumn(nameof(PurchaseOrderProductRecord.Quantity)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderProductRecord.UnitPrice)).AsDecimal(18, 4).NotNullable();
        }
    }
}
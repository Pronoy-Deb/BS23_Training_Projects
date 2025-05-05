using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Mapping
{
    public class PurchaseOrderBuilder : NopEntityBuilder<Domain.PurchaseOrder>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.PurchaseOrder.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Domain.PurchaseOrder.OrderDate)).AsDateTime().NotNullable()
                .WithColumn(nameof(Domain.PurchaseOrder.SupplierId)).AsInt32().NotNullable()
                .ForeignKey("FK_PurchaseOrder_Supplier", nameof(SuppliersRecord), "Id")
                .WithColumn(nameof(Domain.PurchaseOrder.SupplierName)).AsString(400).NotNullable()
                .WithColumn(nameof(Domain.PurchaseOrder.TotalAmount)).AsDecimal(18, 4).NotNullable();
        }
    }
}
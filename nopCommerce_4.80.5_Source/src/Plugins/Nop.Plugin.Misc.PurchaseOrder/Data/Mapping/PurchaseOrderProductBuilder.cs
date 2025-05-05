using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Mapping
{
    public class PurchaseOrderProductBuilder : NopEntityBuilder<PurchaseOrderProduct>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PurchaseOrderProduct.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(PurchaseOrderProduct.PurchaseOrderId)).AsInt32().NotNullable()
                .ForeignKey<Domain.PurchaseOrder>(onDelete: Rule.Cascade)
                .WithColumn(nameof(PurchaseOrderProduct.ProductId)).AsInt32().NotNullable()
                .ForeignKey("FK_PurchaseOrderProduct_Product", "Product", "Id")
                .WithColumn(nameof(PurchaseOrderProduct.Quantity)).AsInt32().NotNullable()
                .WithColumn(nameof(PurchaseOrderProduct.UnitPrice)).AsDecimal(18, 4).NotNullable()
                .WithColumn(nameof(PurchaseOrderProduct.ProductName)).AsString(400).NotNullable()
                .WithColumn(nameof(PurchaseOrderProduct.ProductSku)).AsString(400).NotNullable()
                .WithColumn(nameof(PurchaseOrderProduct.PictureThumbnailUrl)).AsString(int.MaxValue).Nullable();
        }
    }
}
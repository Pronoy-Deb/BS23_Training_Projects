using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Data.Mapping;

public class ProductSupplierBuilder : NopEntityBuilder<ProductSupplier>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ProductSupplier.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(ProductSupplier.ProductId)).AsInt32().NotNullable()
            .WithColumn(nameof(ProductSupplier.SupplierId)).AsInt32().NotNullable();
    }
}

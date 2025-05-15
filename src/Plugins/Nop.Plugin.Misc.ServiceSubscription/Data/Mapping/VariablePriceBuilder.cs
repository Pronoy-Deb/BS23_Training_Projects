using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ServiceSubscription.Domain;

namespace Nop.Plugin.Misc.ServiceSubscription.Data.Mapping
{
    public class VariablePriceBuilder : NopEntityBuilder<VariablePrice>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(VariablePrice.Id)).AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(VariablePrice.ProductId)).AsInt32().NotNullable()
                .ForeignKey(nameof(Product), nameof(Product.Id))
                .WithColumn(nameof(VariablePrice.CustomerId)).AsInt32().NotNullable()
                .ForeignKey(nameof(Customer), nameof(Customer.Id))
                .WithColumn(nameof(VariablePrice.Price)).AsDecimal(18, 4).NotNullable()
                .WithColumn(nameof(VariablePrice.StartDateTimeUtc)).AsDateTime2().Nullable()
                .WithColumn(nameof(VariablePrice.EndDateTimeUtc)).AsDateTime2().Nullable()
                .WithColumn(nameof(VariablePrice.CreatedOnUtc)).AsDateTime2().NotNullable()
                .WithColumn(nameof(VariablePrice.UpdatedOnUtc)).AsDateTime2().NotNullable();
        }
    }
}
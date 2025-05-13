using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.ServiceSubscription.Domain;

namespace Nop.Plugin.Misc.ServiceSubscription.Data.Migrations
{
    [NopSchemaMigration("2025/05/07 04:21:00", "ServiceSubscription base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<VariablePrice>();

            // Add unique constraint
            Create.Index("IX_VariablePrice_ProductId_CustomerId")
                .OnTable(nameof(VariablePrice))
                .OnColumn(nameof(VariablePrice.ProductId)).Ascending()
                .OnColumn(nameof(VariablePrice.CustomerId)).Ascending()
                .WithOptions().Unique();
        }
    }
}
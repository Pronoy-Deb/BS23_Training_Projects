using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Migrations
{
    [NopSchemaMigration("2025/05/05 04:21:00", "PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Domain.PurchaseOrder>();
            Create.TableFor<PurchaseOrderProduct>();
        }
    }
}
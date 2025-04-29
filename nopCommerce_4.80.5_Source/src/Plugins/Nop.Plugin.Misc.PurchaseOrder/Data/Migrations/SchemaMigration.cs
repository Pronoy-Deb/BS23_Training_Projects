using FluentMigrator;
using Nop.Plugin.Misc.PurchaseOrder.Domain;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Migrations
{
    [NopSchemaMigration("2025/04/29 12:00:00", "PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<PurchaseOrderRecord>();
        }
    }
}
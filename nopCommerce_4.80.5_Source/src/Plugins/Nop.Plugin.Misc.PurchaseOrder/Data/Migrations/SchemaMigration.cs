using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Data.Migrations
{
    [NopSchemaMigration("2025/05/02 06:10:00", "PurchaseOrder base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            //IfDatabase("sqlserver").Delete.Column("OrderNumber").FromTable("PurchaseOrderRecord");

            Create.TableFor<PurchaseOrderRecord>();
            // Create new tables
            Create.TableFor<PurchaseOrderProductRecord>();

            // Add OrderNumber column
            //Alter.Table("PurchaseOrderRecord")
            //    .AddColumn("OrderNumber").AsString(50).NotNullable().WithDefaultValue("");
        }
    }
}
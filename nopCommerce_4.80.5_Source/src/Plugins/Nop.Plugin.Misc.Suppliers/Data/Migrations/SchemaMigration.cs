using FluentMigrator;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Data.Migrations;

using Nop.Data.Extensions;
using Nop.Data.Migrations;

[NopSchemaMigration("2025/04/18 05:20:00", "Suppliers base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SuppliersRecord>();
    }
}
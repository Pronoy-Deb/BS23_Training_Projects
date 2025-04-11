using FluentMigrator;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Data.Migrations;

using Nop.Data.Extensions;
using Nop.Data.Migrations;

[NopSchemaMigration("2025/04/11 06:50:00", "Suppliers base schema", MigrationProcessType.Installation)]
public class SchemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<SuppliersRecord>();
    }
}
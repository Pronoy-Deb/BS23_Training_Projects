using FluentMigrator;
using Nop.Plugin.Misc.ProductViewTracker.Domain;

namespace Nop.Plugin.Misc.ProductViewTracker.Data.Migrations;

using Nop.Data.Extensions;
using Nop.Data.Migrations;

[NopSchemaMigration("2020/05/27 08:40:55:1687541", "Other.ProductViewTracker base schema", MigrationProcessType.Installation)]
public class SchemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<ProductViewTrackerRecord>();
    }
}
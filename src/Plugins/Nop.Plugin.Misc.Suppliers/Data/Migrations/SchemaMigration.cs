using FluentMigrator;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Data.Extensions;
using Nop.Data.Migrations;

namespace Nop.Plugin.Misc.Suppliers.Data.Migrations;

[NopSchemaMigration("2025/04/22 04:00:00", "Suppliers base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<SuppliersRecord>();

        Create.TableFor<ProductSupplier>();

        Create.ForeignKey("FK_ProductSupplier_ProductId_Product_Id")
            .FromTable("ProductSupplier").ForeignColumn("ProductId")
            .ToTable("Product").PrimaryColumn("Id");
    }
}
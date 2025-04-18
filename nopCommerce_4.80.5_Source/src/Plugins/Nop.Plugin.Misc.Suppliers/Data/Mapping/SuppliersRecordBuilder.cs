using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Data.Mapping;

public class SuppliersRecordBuilder : NopEntityBuilder<SuppliersRecord>
{
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
               .WithColumn(nameof(SuppliersRecord.Id)).AsInt32().PrimaryKey().Identity()
               .WithColumn(nameof(SuppliersRecord.Name)).AsString(400).NotNullable()
               .WithColumn(nameof(SuppliersRecord.Email)).AsString(200).Nullable()
               .WithColumn(nameof(SuppliersRecord.Description)).AsString(400).Nullable()
               .WithColumn(nameof(SuppliersRecord.AdminComment)).AsString(400).Nullable()
               .WithColumn(nameof(SuppliersRecord.PictureId)).AsInt32().Nullable()
               .WithColumn(nameof(SuppliersRecord.Active)).AsBoolean().Nullable();
    }
}
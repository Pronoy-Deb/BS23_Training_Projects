using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Models;

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
               .WithColumn(nameof(SuppliersRecord.IsActive)).AsBoolean().Nullable();
               //.WithColumn(nameof(SuppliersRecord.DisplayOrder)).AsInt32().WithDefaultValue(0)
               // .WithColumn(nameof(SuppliersRecord.MetaTitle)).AsString(400).Nullable()
               // .WithColumn(nameof(SuppliersRecord.MetaKeywords)).AsString(400).Nullable()
               // .WithColumn(nameof(SuppliersRecord.MetaDescription)).AsString(4000).Nullable();
    }
}
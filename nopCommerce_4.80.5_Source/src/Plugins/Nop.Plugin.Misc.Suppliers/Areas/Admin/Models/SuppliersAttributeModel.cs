using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

/// <summary>
/// Represents a vendor attribute model
/// </summary>
public partial record SuppliersAttributeModel : BaseNopEntityModel, ILocalizedModel<SuppliersAttributeLocalizedModel>
{

    public SuppliersAttributeModel()
    {
        Locales = new List<SuppliersAttributeLocalizedModel>();
        SuppliersAttributeValueSearchModel = new SuppliersAttributeValueSearchModel();
    }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.AttributeControlType")]
    public int AttributeControlTypeId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.AttributeControlType")]
    public string AttributeControlTypeName { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<SuppliersAttributeLocalizedModel> Locales { get; set; }

    public SuppliersAttributeValueSearchModel SuppliersAttributeValueSearchModel { get; set; }

}

public partial record SuppliersAttributeLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Fields.Name")]
    public string Name { get; set; }
}
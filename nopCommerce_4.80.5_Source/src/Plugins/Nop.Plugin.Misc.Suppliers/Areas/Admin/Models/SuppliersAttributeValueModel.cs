using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

/// <summary>
/// Represents a vendor attribute value model
/// </summary>
public partial record SuppliersAttributeValueModel : BaseNopEntityModel, ILocalizedModel<SuppliersAttributeValueLocalizedModel>
{

    public SuppliersAttributeValueModel()
    {
        Locales = new List<SuppliersAttributeValueLocalizedModel>();
    }

    public int AttributeId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Values.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Values.Fields.IsPreSelected")]
    public bool IsPreSelected { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Values.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<SuppliersAttributeValueLocalizedModel> Locales { get; set; }

}

public partial record SuppliersAttributeValueLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersAttributes.Values.Fields.Name")]
    public string Name { get; set; }
}
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

/// <summary>
/// Represents a vendor attribute value search model
/// </summary>
public partial record SuppliersAttributeValueSearchModel : BaseSearchModel
{
    public int SuppliersAttributeId { get; set; }

}
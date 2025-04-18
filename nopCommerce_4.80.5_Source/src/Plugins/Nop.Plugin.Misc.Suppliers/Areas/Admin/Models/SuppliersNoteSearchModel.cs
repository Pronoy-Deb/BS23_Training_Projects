using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

/// <summary>
/// Represents a vendor note search model
/// </summary>
public partial record SuppliersNoteSearchModel : BaseSearchModel
{
    public int SupplierId { get; set; }

}
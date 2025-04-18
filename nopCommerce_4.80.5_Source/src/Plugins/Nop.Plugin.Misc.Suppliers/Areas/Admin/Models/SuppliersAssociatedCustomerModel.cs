using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

/// <summary>
/// Represents a vendor associated customer model
/// </summary>
public partial record SuppliersAssociatedCustomerModel : BaseNopEntityModel
{
    public string Email { get; set; }

}
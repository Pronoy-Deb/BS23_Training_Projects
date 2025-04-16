using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Models;
public record SuppliersModel: BaseNopEntityModel
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

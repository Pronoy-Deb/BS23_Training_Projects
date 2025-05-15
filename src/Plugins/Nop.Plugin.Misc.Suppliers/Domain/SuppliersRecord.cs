using Nop.Core;
using Nop.Core.Domain.Localization;
namespace Nop.Plugin.Misc.Suppliers.Domain;
public class SuppliersRecord : BaseEntity, ILocalizedEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public int PictureId { get; set; }
    public string AdminComment { get; set; }
    public bool Active { get; set; } = true;
}

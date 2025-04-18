using Nop.Core;
namespace Nop.Plugin.Misc.Suppliers.Domain;
public class SuppliersRecord : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }    
    public string Description { get; set; }
    public int PictureId { get; set; }
    public string AdminComment { get; set; }
    public bool Active { get; set; }
}
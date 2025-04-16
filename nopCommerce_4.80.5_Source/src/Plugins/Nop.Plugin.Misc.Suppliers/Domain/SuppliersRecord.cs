using Nop.Core;
namespace Nop.Plugin.Misc.Suppliers.Domain;
public class SuppliersRecord : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }    
    public bool IsActive { get; set; }
}
using Nop.Core;
namespace Nop.Plugin.Misc.ProductViewTracker.Domain;

public class ProductViewTrackerRecord : BaseEntity
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int CustomerId { get; set; }
    public string IpAddress { get; set; }
    public bool IsRegistered { get; set; }
}
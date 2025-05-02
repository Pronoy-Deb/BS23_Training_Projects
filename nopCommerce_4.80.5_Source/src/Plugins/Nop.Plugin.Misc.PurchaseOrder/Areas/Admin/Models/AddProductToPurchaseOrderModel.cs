using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;

public record AddProductToPurchaseOrderModel : BaseNopModel
{
    public AddProductToPurchaseOrderModel()
    {
        SelectedProductIds = new List<int>();
    }

    public IList<int> SelectedProductIds { get; set; }

    public int PurchaseOrderId { get; set; }

    public string ButtonId { get; set; }

    public string FormId { get; set; }
}

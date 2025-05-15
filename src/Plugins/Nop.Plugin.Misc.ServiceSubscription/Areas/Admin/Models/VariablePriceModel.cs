using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Models
{
    public record VariablePriceModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Price { get; set; }
    }

    public record VariablePriceListModel : BasePagedListModel<VariablePriceModel>
    {
        public List<VariablePriceModel> Data { get; set; }
    }
}
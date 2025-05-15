using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Models
{
    public record ServiceSubscriptionModel : BaseNopEntityModel
    {
        public ProductListModel ProductListModel { get; set; }
    }
}
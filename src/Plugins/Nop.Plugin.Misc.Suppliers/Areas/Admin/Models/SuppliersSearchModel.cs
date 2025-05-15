using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public record SuppliersSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
        public string SearchName { get; init; }
        [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
        public string SearchEmail { get; init; }
        [NopResourceDisplayName("Admin.Suppliers.Fields.IsActive")]
        public bool? Active { get; internal set; }
    }
}

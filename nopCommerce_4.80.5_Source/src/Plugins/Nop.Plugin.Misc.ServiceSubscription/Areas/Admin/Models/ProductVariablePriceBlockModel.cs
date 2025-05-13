using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Models
{
    public class ProductVariablePriceBlockModel
    {
        public ProductModel ProductModel { get; set; }
        public List<SelectListItem> AvailableCustomers { get; set; }
        public string HideBlockAttributeName { get; set; }
        public bool HideBlock { get; set; }
        public int SelectedCustomerId { get; set; }
        public decimal VariablePrice { get; set; }
    }
}
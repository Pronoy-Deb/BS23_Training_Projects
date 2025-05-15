using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public record ProductSupplierModel : BaseNopModel
    {
        public ProductSupplierModel()
        {
            AvailableSuppliers = new List<SelectListItem>();
        }

        public int ProductId { get; set; }

        public AssignedSupplierModel AssignedSupplier { get; set; }

        public List<SelectListItem> AvailableSuppliers { get; set; }
    }

    public record AssignedSupplierModel : BaseNopEntityModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
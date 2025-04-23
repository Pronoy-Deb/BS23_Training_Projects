using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public class ProductSupplierModel
    {
        public int ProductId { get; set; }
        public int SelectedSupplierId { get; set; }
        public List<SelectListItem> AvailableSuppliers { get; set; } = new();
        public List<AssignedSupplierModel> AssignedSuppliers { get; set; } = new();
    }
}

public class AssignedSupplierModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
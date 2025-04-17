using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.Suppliers.Models;
public record SuppliersModel: BaseNopEntityModel
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }

    // Display properties
    public int DisplayOrder { get; set; }

    // SEO properties
    public string MetaTitle { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }

    // For notes
    public IList<SupplierNote> SupplierNotes { get; set; }
}

public class SupplierNote
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public string Note { get; set; }
    public DateTime CreatedOn { get; set; }
}

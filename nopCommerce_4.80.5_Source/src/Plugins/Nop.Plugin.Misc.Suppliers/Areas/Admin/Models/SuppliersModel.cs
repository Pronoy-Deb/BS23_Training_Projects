using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models; 
public record SuppliersModel : BaseNopEntityModel, ILocalizedModel<SuppliersLocalizedModel>
{
    public SuppliersModel()
    {
        Locales = new List<SuppliersLocalizedModel>();
    }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Active")]
    public bool Active { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.PictureId")]
    public int PictureId { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.AdminComment")]
    public string AdminComment { get; set; }
    public IList<SuppliersLocalizedModel> Locales { get; set; }
}

public partial record SuppliersLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }
    [Required]
    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }
    [Required]
    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }
    [Required]
    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }
}
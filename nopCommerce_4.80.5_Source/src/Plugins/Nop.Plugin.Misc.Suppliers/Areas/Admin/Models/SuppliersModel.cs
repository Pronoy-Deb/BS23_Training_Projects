using System.ComponentModel.DataAnnotations;
using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models; 
public record SuppliersModel : BaseNopEntityModel, ILocalizedModel<SuppliersLocalizedModel>
{
    public SuppliersModel()
    {
        if (PageSize < 1)
            PageSize = 5;

        SuppliersAttributes = new List<SuppliersAttributeModel>();
        Address = new AddressModel();
        Locales = new List<SuppliersLocalizedModel>();
        AssociatedCustomers = new List<SuppliersAssociatedCustomerModel>();
        SuppliersNoteSearchModel = new SuppliersNoteSearchModel();
    }

    [NopResourceDisplayName("Admin.Suppliers.Fields.SuppliersAttributes")]
    public IList<SuppliersAttributeModel> SuppliersAttributes { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Address")]
    public AddressModel Address { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }

    [DataType(DataType.EmailAddress)]
    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }

    [UIHint("Picture")]
    [NopResourceDisplayName("Admin.Suppliers.Fields.PictureId")]
    public int PictureId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.AdminComment")]
    public string AdminComment { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Active")]
    public bool Active { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaKeywords")]
    public string MetaKeywords { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaDescription")]
    public string MetaDescription { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaTitle")]
    public string MetaTitle { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.SeName")]
    public string SeName { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PageSize")]
    public int PageSize { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.AllowCustomersToSelectPageSize")]
    public bool AllowCustomersToSelectPageSize { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PageSizeOptions")]
    public string PageSizeOptions { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PmCustomerId")]
    public int? PmCustomerId { get; set; }
    public string PmCustomerInfo { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PriceRangeFiltering")]
    public bool PriceRangeFiltering { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PriceFrom")]
    public decimal PriceFrom { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.PriceTo")]
    public decimal PriceTo { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.ManuallyPriceRange")]
    public bool ManuallyPriceRange { get; set; }

    public List<SuppliersAttributeModel> VendorAttributes { get; set; }

    public IList<SuppliersLocalizedModel> Locales { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.AssociatedCustomerEmails")]
    public IList<SuppliersAssociatedCustomerModel> AssociatedCustomers { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.SuppliersNotes.Fields.Note")]
    public string AddSuppliersNoteMessage { get; set; }

    public SuppliersNoteSearchModel SuppliersNoteSearchModel { get; set; }

    public string PrimaryStoreCurrencyCode { get; set; }

    public partial record SuppliersAttributeModel : BaseNopEntityModel
    {
        public SuppliersAttributeModel()
        {
            Values = new List<SuppliersAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<SuppliersAttributeValueModel> Values { get; set; }
    }

    public partial record SuppliersAttributeValueModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}

public partial record SuppliersLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Description")]
    public string Description { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaKeywords")]
    public string MetaKeywords { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaDescription")]
    public string MetaDescription { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.MetaTitle")]
    public string MetaTitle { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.SeName")]
    public string SeName { get; set; }
}

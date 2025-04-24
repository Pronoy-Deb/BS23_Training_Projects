using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using Nop.Plugin.Misc.Suppliers.Components;

namespace Nop.Plugin.Misc.Suppliers;

public class SuppliersPlugin : BasePlugin, IWidgetPlugin
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;

    public SuppliersPlugin(IPermissionService permissionService, ILocalizationService localizationService)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
    }

    private Dictionary<string, string> _resourceString = new Dictionary<string, string>
    {
        ["Admin.Suppliers"] = "Suppliers",
        ["Admin.Suppliers.AddNew"] = "Add a New Supplier",
        ["Admin.Suppliers.EditSuppliersDetails"] = "Edit Supplier Details",
        ["Admin.Suppliers.BackToList"] = "Back to Suppliers List",

        ["Admin.Suppliers.Fields.Name"] = "Name",
        ["Admin.Suppliers.Fields.Name.Hint"] = "Enter Supplier Name.",
        ["Admin.Suppliers.Fields.Email"] = "Supplier Email",
        ["Admin.Suppliers.Fields.Email.Hint"] = "Enter Supplier Email.",

        ["Admin.Suppliers.Fields.Name.Show"] = "Supplier Name",
        ["Admin.Suppliers.Fields.Email.Show"] = "Supplier Email",
        ["Admin.Suppliers.Fields.Name.Show.Hint"] = "Search by Supplier Name",
        ["Admin.Suppliers.Fields.Email.Show.Hint"] = "Search by Supplier Email",

        ["Admin.Suppliers.Info"] = "Supplier Info",
        ["Admin.Suppliers.Fields.Description"] = "Description",
        ["Admin.Suppliers.Fields.Active"] = "Active",
        ["Admin.Suppliers.Fields.PictureId"] = "Picture",
        ["Admin.Suppliers.Fields.AdminComment"] = "Admin Comment",
        ["Admin.Suppliers.Address"] = "Address (optional)",
        ["Admin.Suppliers.Fields.SeName"] = "Search engine friendly page name",
        ["Admin.Suppliers.Fields.Metatitle"] = "Meta title",
        ["Admin.Suppliers.Fields.MetaKeywords"] = "Meta keywords",
        ["Admin.Suppliers.Fields.MetaDescription"] = "Meta description",

        ["Plugins.Misc.Suppliers.Product.Suppliers"] = "Supplier",
        ["Admin.Catalog.Products.Productsupplier.Savebeforeedit"] = "Please save the product first!",
    };

    public bool HideInWidgetList => false;

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

        await base.UpdateAsync(currentVersion, targetVersion);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { "admin_product_details_block" });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductSupplierViewComponent);
    }
}

public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    private readonly IPermissionService _permissionService;

    public EventConsumer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
            return;

        eventMessage.RootMenuItem.InsertAfter("Help",
            new AdminMenuItem
            {
                SystemName = "Misc.Suppliers",
                Title = "Suppliers",
                Url = eventMessage.GetMenuItemUrl("Suppliers", "List"),
                IconClass = "far fa-dot-circle",
                Visible = true,
            });
    }
}
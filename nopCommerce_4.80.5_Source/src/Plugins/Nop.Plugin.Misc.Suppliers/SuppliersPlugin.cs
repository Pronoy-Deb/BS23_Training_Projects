using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.Suppliers;

public class SuppliersPlugin : BasePlugin
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;

    public SuppliersPlugin(IPermissionService permissionService, ILocalizationService localizationService)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
    }

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {

            ["Admin.Suppliers"] = "Suppliers",
            ["Admin.Suppliers.AddNew"] = "Add a New Supplier",
            ["Admin.Suppliers.EditDetails"] = "Edit Supplier Details",
            ["Admin.Suppliers.BackToList"] = "Back to Suppliers List",
             
            ["Admin.Suppliers.Fields.Name"] = "Name",
            ["Admin.Suppliers.Fields.Name.Hint"] = "Enter Supplier Name.",
            ["Admin.Suppliers.Fields.Email"] = "Supplier Email",
            ["Admin.Suppliers.Fields.Email.Hint"] = "Enter Supplier Email.",

            ["Admin.Suppliers.Fields.Name"] = "Supplier Name",
            ["Admin.Suppliers.Fields.Email"] = "Supplier Email",
            ["Admin.Suppliers.Fields.Name.Hint"] = "Search by Supplier Name",
            ["Admin.Suppliers.Fields.Email.Hint"] = "Search by Supplier Email",

            ["Admin.Suppliers.Info"] = "Supplier Info",
            ["Admin.Suppliers.Fields.Description"] = "Description",
            ["Admin.Suppliers.Fields.Active"] = "Active",
            ["Admin.Suppliers.Fields.PictureId"] = "Picture",
            ["Admin.Suppliers.Fields.AdminComment"] = "Admin Comment",

        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
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

        eventMessage.RootMenuItem.InsertBefore("Local plugins",
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

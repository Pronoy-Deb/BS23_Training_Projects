//using Nop.Plugin.Misc.ServiceSubscription.Components;

using Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Components;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.ServiceSubscription;
public class ServiceSubscriptionPlugin : BasePlugin, IWidgetPlugin
{
    private readonly ILocalizationService _localizationService;
    public ServiceSubscriptionPlugin(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }
    private Dictionary<string, string> _resourceString = new Dictionary<string, string>
    {
        ["Admin.ServiceSubscription"] = "ServiceSubscription"
    };
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

    public bool HideInWidgetList => false;

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        //return Task.FromResult<IList<string>>(new List<string> { AdminWidgetZones.ProductDetailsBlock });
        //return Task.FromResult<IList<string>>(new List<string>
        //{
        //    "admin_product_details_block" // Try this first
        //    //"Admin.Product.Details.Block", // Fallback for newer versions
        //    //"product_details_block"        // Fallback for older versions
        //});
        return Task.FromResult<IList<string>>(new List<string> { "admin_product_details_block" });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductVariablePriceBlockViewComponent);
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
            SystemName = "Misc.ServiceSubscription",
            Title = "Service Subscription",
            Url = eventMessage.GetMenuItemUrl("ServiceSubscription", "List"),
            IconClass = "fas fa-repeat",
            Visible = true,
        });
    }
}
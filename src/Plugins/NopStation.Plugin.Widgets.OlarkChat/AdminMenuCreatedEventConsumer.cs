using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Menu;
using NopStation.Plugin.Misc.Core.Infrastructure;
using NopStation.Plugin.Misc.Core;

namespace NopStation.Plugin.Widgets.OlarkChat;

public class AdminMenuCreatedEventConsumer : IConsumer<AdminMenuEvent>
{
    private readonly ILocalizationService _localizationService;
    private readonly IPermissionService _permissionService;

    public AdminMenuCreatedEventConsumer(ILocalizationService localizationService, 
        IPermissionService permissionService)
    {
        _localizationService = localizationService;
        _permissionService = permissionService;
    }

    public async Task HandleEventAsync(AdminMenuEvent createdEvent)
    {
        var menu = new NopStationAdminMenuItem()
        {
            Visible = true,
            IconClass = "far fa-dot-circle",
            Title = await _localizationService.GetResourceAsync("Admin.NopStation.OlarkChat.Menu.OlarkChat")
        };

        if (await _permissionService.AuthorizeAsync(OlarkChatPermissionProvider.MANAGE_CONFIGURATION))
        {
            var settings = new AdminMenuItem()
            {
                Visible = true,
                IconClass = "far fa-circle",
                Url = "~/Admin/OlarkChat/Configure",
                Title = await _localizationService.GetResourceAsync("Admin.NopStation.OlarkChat.Menu.Configuration"),
                SystemName = "OlarkChat.Configuration"
            };
            menu.ChildNodes.Add(settings);
        }

        if (await _permissionService.AuthorizeAsync(CorePermissionProvider.SHOW_DOCUMENTATIONS))
        {
            var documentation = new AdminMenuItem()
            {
                Title = await _localizationService.GetResourceAsync("Admin.NopStation.Common.Menu.Documentation"),
                Url = "https://www.nop-station.com/olark-chat-documentation?utm_source=admin-panel&utm_medium=products&utm_campaign=algolia-search",
                Visible = true,
                IconClass = "far fa-circle",
                OpenUrlInNewTab = true
            };
            menu.ChildNodes.Add(documentation);
        }

        createdEvent.PluginChildNodes.Add(menu);
    }
}

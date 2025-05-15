using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using NopStation.Plugin.Widgets.OlarkChat.Components;
using Nop.Services.Localization;
using NopStation.Plugin.Misc.Core.Services;
using Nop.Web.Framework.Infrastructure;

namespace NopStation.Plugin.Widgets.OlarkChat;

public class OlarkChatPlugin : BasePlugin, IWidgetPlugin, INopStationPlugin
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly IWebHelper _webHelper;
    private readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public OlarkChatPlugin(
        ISettingService settingService,
        IWebHelper webHelper,
        ILocalizationService localizationService)
    {
        _settingService = settingService;
        _webHelper = webHelper;
        _localizationService = localizationService;
    }

    #endregion

    #region Properties

    public bool HideInWidgetList => false;

    #endregion

    #region Methods

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.Footer });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(OlarkChatViewComponent);
    }

    public override async Task InstallAsync()
    {
        await this.InstallPluginAsync();
        var settings = new OlarkChatSettings
        {
            SiteId = "",
            WidgetPosition = "right",
            EnableMobile = true
        };

        await _settingService.SaveSettingAsync(settings);
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await this.UninstallPluginAsync();
        await _settingService.DeleteSettingAsync<OlarkChatSettings>();
        await base.UninstallAsync();
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/OlarkChat/OlarkChatConfigure";
    }

    public IDictionary<string, string> GetPluginResources()
    {
        var list = new Dictionary<string, string>
        {
            ["Admin.NopStation.OlarkChat.Configuration"] = "OlarkChat settings",
            ["Admin.NopStation.OlarkChat.Menu.OlarkChat"] = "OlarkChat",
            ["Admin.NopStation.OlarkChat.Menu.Configuration"] = "Configuration",

            ["Admin.NopStation.OlarkChat.Configuration.Fields.SiteId"] = "Site ID",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.SiteId.Hint"] = "Enter your Olark site ID. You can find this in your Olark dashboard under Installation settings.",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.WidgetPosition"] = "Widget position",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.WidgetPosition.Hint"] = "Determine whether the chat widget appears on the left or right side of the screen.",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.EnableMobile"] = "Enable on mobile",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.EnableMobile.Hint"] = "Check to enable the chat widget on mobile devices.",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.UseDarkTheme"] = "Use dark theme",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.UseDarkTheme.Hint"] = "Check to enable Olark's dark color scheme for the chat widget.",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.CustomScript"] = "Custom script",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.CustomScript.Hint"] = "Enter any custom JavaScript code you want to run with the Olark widget.",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.UseScriptMode"] = "Use script mode",
            ["Admin.NopStation.OlarkChat.Configuration.Fields.UseScriptMode.Hint"] = "Enable to inject script instead of iframe-based integration.",

            ["Admin.NopStation.OlarkChat.Instructions.Title"] = "Olark Chat Setup Instructions",
            ["Admin.NopStation.OlarkChat.Instructions.Step1"] = "Sign up for an account at {0}",
            ["Admin.NopStation.OlarkChat.Instructions.Step2"] = "Get your Site ID from the Installation page in your Olark dashboard",
            ["Admin.NopStation.OlarkChat.Instructions.Step3"] = "Enter the Site ID in the field above and configure your preferences",
            ["Admin.NopStation.OlarkChat.Instructions.Step4"] = "Save changes - the chat widget will now appear in your store",
            ["Admin.NopStation.OlarkChat.Instructions.Note"] = "Note: You may need to clear your cache after saving changes for them to take effect immediately."
        };

        return list;
    }

    #endregion
}
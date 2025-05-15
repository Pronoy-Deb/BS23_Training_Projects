using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using NopStation.Plugin.Widgets.OlarkChat.Components;
using Nop.Services.Localization;
using NopStation.Plugin.Misc.Core.Services;

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
        return Task.FromResult<IList<string>>(new List<string> { "footer" });
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
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(GetPluginResources()));
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await this.UninstallPluginAsync();
        await _settingService.DeleteSettingAsync<OlarkChatSettings>();
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(GetPluginResources()));
        await base.UpdateAsync(currentVersion, targetVersion);
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/OlarkChat/OlarkChatConfigure";
    }

    public IDictionary<string, string> GetPluginResources()
    {
        var list = new Dictionary<string, string>
        {
            ["Admin.NopStation.OlarkChat.Fields.SiteId"] = "Site ID",
            ["Admin.NopStation.OlarkChat.Fields.SiteId.Hint"] = "Enter your Olark site ID. You can find this in your Olark dashboard under Installation settings.",
            ["Admin.NopStation.OlarkChat.Fields.SiteId.Required"] = "Site ID is required",

            ["Admin.NopStation.OlarkChat.Fields.WidgetPosition"] = "Widget position",
            ["Admin.NopStation.OlarkChat.Fields.WidgetPosition.Hint"] = "Determine whether the chat widget appears on the left or right side of the screen.",

            ["Admin.NopStation.OlarkChat.Fields.EnableMobile"] = "Enable on mobile",
            ["Admin.NopStation.OlarkChat.Fields.EnableMobile.Hint"] = "Check to enable the chat widget on mobile devices.",

            ["Admin.NopStation.OlarkChat.Fields.UseDarkTheme"] = "Use dark theme",
            ["Admin.NopStation.OlarkChat.Fields.UseDarkTheme.Hint"] = "Check to enable Olark's dark color scheme for the chat widget.",

            ["Admin.NopStation.OlarkChat.Fields.CustomScript"] = "Custom script",
            ["Admin.NopStation.OlarkChat.Fields.CustomScript.Hint"] = "Enter any custom JavaScript code you want to run with the Olark widget.",

            ["Admin.NopStation.OlarkChat.Instructions.Title"] = "Olark Chat Setup Instructions",
            ["Admin.NopStation.OlarkChat.Instructions.Step1"] = "Sign up for an account at {0}",
            ["Admin.NopStation.OlarkChat.Instructions.Step2"] = "Get your Site ID from the Installation page in your Olark dashboard",
            ["Admin.NopStation.OlarkChat.Instructions.Step3"] = "Enter the Site ID in the field above and configure your preferences",
            ["Admin.NopStation.OlarkChat.Instructions.Step4"] = "Save changes - the chat widget will now appear in your store",
            ["Admin.NopStation.OlarkChat.Instructions.Note"] = "Note: You may need to clear your cache after saving changes for them to take effect immediately.",

            ["Admin.NopStation.OlarkChat.Basic"] = "Basic Settings",
            ["Admin.NopStation.OlarkChat.Advanced"] = "Advanced Settings",

            ["Enums.NopStation.Plugin.Widgets.OlarkChat.WidgetPosition.Left"] = "Left",
            ["Enums.NopStation.Plugin.Widgets.OlarkChat.WidgetPosition.Right"] = "Right"

        };

        return list;
    }

    #endregion
}
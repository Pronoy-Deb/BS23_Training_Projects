using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Plugin.Widgets.OlarkChat.Models;
using Nop.Plugin.Widgets.OlarkChat.Components;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.OlarkChat
{
    public class OlarkChatPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public OlarkChatPlugin(
            ISettingService settingService,
            IWebHelper webHelper,
            ILocalizationService localizationService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }

        private Dictionary<string, string> _resourceString = new()
        {
            ["plugins.widgets.olarkchat.fields.siteid"] = "Site ID",
            ["plugins.widgets.olarkchat.fields.siteid.Hint"] = "Enter the site ID",
            ["plugins.widgets.olarkchat.fields.widgetposition"] = "Widget position",
            ["plugins.widgets.olarkchat.fields.widgetposition.Hint"] = "Select the widget position",
            ["plugins.widgets.olarkchat.fields.enablemobile"] = "Enable mobile",
            ["plugins.widgets.olarkchat.fields.enablemobile.Hint"] = "Enable/Disable mobile",
            ["plugins.widgets.olarkchat.instructions.title"] = "Olark Chat Setup Instructions",
            ["plugins.widgets.olarkchat.instructions.step1"] = "Sign up for an account at {0}",
            ["plugins.widgets.olarkchat.instructions.step2"] =
                "Get your Site ID from the Installation page in your Olark dashboard",
            ["plugins.widgets.olarkchat.instructions.step3"] =
                "Enter the Site ID in the field above and configure your preferences",
            ["plugins.widgets.olarkchat.instructions.step4"] =
                "Save changes - the chat widget will now appear in your store",
            ["plugins.widgets.olarkchat.instructions.note"] =
                "Note: You may need to clear your cache after saving changes for them to take effect immediately.",
            ["plugins.widgets.olarkchat.fields.usedarktheme"] = "Enable dark theme",
            ["plugins.widgets.olarkchat.fields.usedarktheme.Hint"] = "Check to enable Olark's dark theme"
        };

        public bool HideInWidgetList => false;

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { "footer" });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetOlarkChatViewComponent);
        }

        public override async Task InstallAsync()
        {
            var settings = new OlarkChatSettings
            {
                SiteId = "9930-373-10-1373",
                WidgetPosition = "right",
                EnableMobile = true
            };

            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<OlarkChatSettings>();
            await base.UninstallAsync();
        }

        public override async Task UpdateAsync(string currentVersion, string targetVersion)
        {
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

            await base.UpdateAsync(currentVersion, targetVersion);
        }

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/OlarkChat/OlarkChatConfigure";
        }
    }
}
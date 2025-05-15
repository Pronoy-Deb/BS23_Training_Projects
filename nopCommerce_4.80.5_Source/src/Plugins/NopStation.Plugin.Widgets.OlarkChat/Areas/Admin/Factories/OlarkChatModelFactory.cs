using Nop.Services.Configuration;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;

public class OlarkChatModelFactory : IOlarkChatModelFactory
{
    #region Fields

    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public OlarkChatModelFactory(ISettingService settingService)
    {
        _settingService = settingService;
    }

    #endregion

    #region Methods

    public async Task<OlarkChatConfigurationModel> PrepareConfigurationModelAsync()
    {
        var settings = await _settingService.LoadSettingAsync<OlarkChatSettings>();

        var model = new OlarkChatConfigurationModel
        {
            SiteId = settings.SiteId,
            WidgetPosition = settings.WidgetPosition,
            EnableMobile = settings.EnableMobile,
            UseDarkTheme = settings.UseDarkTheme,
            ConfigurationMode = settings.ConfigurationMode ?? "basic",
            CustomScript = settings.CustomScript,
        };

        return model;
    }

    public async Task SaveConfigurationModelAsync(OlarkChatConfigurationModel model)
    {
        var settings = new OlarkChatSettings
        {
            SiteId = model.SiteId,
            WidgetPosition = model.WidgetPosition?.ToLower(),
            EnableMobile = model.EnableMobile,
            UseDarkTheme = model.UseDarkTheme,
            ConfigurationMode = model.ConfigurationMode,
            CustomScript = model.CustomScript
        };

        await _settingService.SaveSettingAsync(settings);
        await _settingService.ClearCacheAsync();
    }

    #endregion
}
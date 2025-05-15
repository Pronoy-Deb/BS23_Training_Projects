using Nop.Core;
using Nop.Services.Configuration;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;

public class OlarkChatModelFactory : IOlarkChatModelFactory
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public OlarkChatModelFactory(ISettingService settingService,
        IStoreContext storeContext)
    {
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    public async Task<ConfigurationModel> PrepareConfigurationModelAsync()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var olarkChatSettings = await _settingService.LoadSettingAsync<OlarkChatSettings>(storeScope);

        var model = new ConfigurationModel
        {
            ActiveStoreScopeConfiguration = storeScope,
            SiteId = olarkChatSettings.SiteId,
            UseScriptMode = olarkChatSettings.UseScriptMode,
            CustomScript = olarkChatSettings.CustomScript,
            WidgetPosition = olarkChatSettings.WidgetPosition,
            UseDarkTheme = olarkChatSettings.UseDarkTheme,
            EnableMobile = olarkChatSettings.EnableMobile,
        };

        if (storeScope > 0)
        {
            model.SiteId_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.SiteId, storeScope);
            model.UseScriptMode_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.UseScriptMode, storeScope);
            model.CustomScript_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.CustomScript, storeScope);
            model.WidgetPosition_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.WidgetPosition, storeScope);
            model.UseDarkTheme_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.UseDarkTheme, storeScope);
            model.EnableMobile_OverrideForStore = await _settingService.SettingExistsAsync(olarkChatSettings, x => x.EnableMobile, storeScope);
        }

        return model;
    }

    #endregion
}
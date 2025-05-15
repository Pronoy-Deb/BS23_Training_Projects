using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Mvc.Filters;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;
using NopStation.Plugin.Misc.Core.Controllers;
using Nop.Services.Configuration;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Controllers;

public class OlarkChatController : NopStationAdminController
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IOlarkChatModelFactory _olarkChatModelFactory;
    private readonly IStoreContext _storeContext;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public OlarkChatController(ILocalizationService localizationService,
        INotificationService notificationService,
        IOlarkChatModelFactory olarkChatModelFactory,
        IStoreContext storeContext,
        ISettingService settingService)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _olarkChatModelFactory = olarkChatModelFactory;
        _storeContext = storeContext;
        _settingService = settingService;
    }

    #endregion

    #region Methods

    [CheckPermission(OlarkChatPermissionProvider.MANAGE_CONFIGURATION)]
    public async Task<IActionResult> Configure()
    {
        var model = await _olarkChatModelFactory.PrepareConfigurationModelAsync();
        return View("~/Plugins/NopStation.Plugin.Widgets.OlarkChat/Areas/Admin/Views/OlarkChat/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(OlarkChatPermissionProvider.MANAGE_CONFIGURATION)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var olarkChatSettings = await _settingService.LoadSettingAsync<OlarkChatSettings>(storeScope);

        olarkChatSettings.SiteId = model.SiteId;
        olarkChatSettings.UseScriptMode = model.UseScriptMode;
        olarkChatSettings.CustomScript = model.CustomScript;
        olarkChatSettings.WidgetPosition = model.WidgetPosition;
        olarkChatSettings.UseDarkTheme = model.UseDarkTheme;
        olarkChatSettings.EnableMobile = model.EnableMobile;

        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.SiteId, model.SiteId_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.UseScriptMode, model.UseScriptMode_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.CustomScript, model.CustomScript_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.WidgetPosition, model.WidgetPosition_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.UseDarkTheme, model.UseDarkTheme_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(olarkChatSettings, x => x.EnableMobile, model.EnableMobile_OverrideForStore, storeScope, false);

        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

        return RedirectToAction("Configure");
    }

    #endregion
}
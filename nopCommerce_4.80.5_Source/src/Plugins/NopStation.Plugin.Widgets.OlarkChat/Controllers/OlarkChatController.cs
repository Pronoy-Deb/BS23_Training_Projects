using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NopStation.Plugin.Widgets.OlarkChat.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace NopStation.Plugin.Widgets.OlarkChat.Controllers;
public class OlarkChatController : BasePluginController
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor
    public OlarkChatController(
        ISettingService settingService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _settingService = settingService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Methods

    private static readonly List<string> _widgetPositions = new() { "right", "left" };
    private static readonly List<SelectListItem> _configurationModes = new()
    {
        new SelectListItem { Text = "Select options", Value = "basic" },
        new SelectListItem { Text = "Paste script", Value = "advanced" }
    };

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SETTINGS)]
    public async Task<IActionResult> OlarkChatConfigure()
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
            AvailableWidgetPositions = _widgetPositions,
            AvailableConfigurationModes = _configurationModes
        };

        return View(model);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SETTINGS)]
    public async Task<IActionResult> OlarkChatConfigure(OlarkChatConfigurationModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableWidgetPositions = _widgetPositions;
            model.AvailableConfigurationModes = _configurationModes;
            return View(model);
        }

        var settings = new OlarkChatSettings
        {
            SiteId = model.SiteId,
            WidgetPosition = model.WidgetPosition,
            EnableMobile = model.EnableMobile,
            UseDarkTheme = model.UseDarkTheme,
            ConfigurationMode = model.ConfigurationMode,
            CustomScript = model.CustomScript
        };

        await _settingService.SaveSettingAsync(settings);
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return RedirectToAction("OlarkChatConfigure");
    }

    #endregion
}
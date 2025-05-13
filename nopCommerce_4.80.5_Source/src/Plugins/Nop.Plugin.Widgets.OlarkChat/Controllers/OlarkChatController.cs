using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.OlarkChat.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.OlarkChat.Controllers
{
    public class OlarkChatController : BasePluginController
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;

        public OlarkChatController(
            ISettingService settingService,
            ILocalizationService localizationService,
            INotificationService notificationService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _notificationService = notificationService;
        }

        private static readonly List<string> _widgetPositions = new(){"right", "left"};

        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        public async Task<IActionResult> OlarkChatConfigure()
        {
            var settings = await _settingService.LoadSettingAsync<OlarkChatSettings>();

            var model = new OlarkChatConfigurationModel
            {
                SiteId = settings.SiteId,
                WidgetPosition = settings.WidgetPosition,
                EnableMobile = settings.EnableMobile,
                UseDarkTheme = settings.UseDarkTheme,
                AvailableWidgetPositions = _widgetPositions
            };

            return View(model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.ADMIN)]
        public async Task<IActionResult> OlarkChatConfigure(OlarkChatConfigurationModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableWidgetPositions = _widgetPositions;
                return View(model);
            }

            var settings = new OlarkChatSettings
            {
                SiteId = model.SiteId,
                WidgetPosition = model.WidgetPosition,
                EnableMobile = model.EnableMobile,
                UseDarkTheme = model.UseDarkTheme
            };

            await _settingService.SaveSettingAsync(settings);
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return RedirectToAction("OlarkChatConfigure");
        }
    }
}
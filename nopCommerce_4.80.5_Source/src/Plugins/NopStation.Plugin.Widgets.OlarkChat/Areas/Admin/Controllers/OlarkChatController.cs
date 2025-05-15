using Microsoft.AspNetCore.Mvc;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Controllers;

public class OlarkChatController : BasePluginController
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IOlarkChatModelFactory _olarkChatModelFactory;

    #endregion

    #region Ctor
    public OlarkChatController(ILocalizationService localizationService,
        INotificationService notificationService,
        IOlarkChatModelFactory olarkChatModelFactory)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _olarkChatModelFactory = olarkChatModelFactory;
    }

    #endregion

    #region Methods

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SETTINGS)]
    public async Task<IActionResult> OlarkChatConfigure()
    {
        var model = await _olarkChatModelFactory.PrepareConfigurationModelAsync();
        return View(model);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SETTINGS)]
    public async Task<IActionResult> OlarkChatConfigure(OlarkChatConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _olarkChatModelFactory.SaveConfigurationModelAsync(model);

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return RedirectToAction("OlarkChatConfigure");
    }

    #endregion
}
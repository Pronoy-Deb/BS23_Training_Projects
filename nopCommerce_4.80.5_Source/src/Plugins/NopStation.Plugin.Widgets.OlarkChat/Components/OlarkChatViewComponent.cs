using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using NopStation.Plugin.Misc.Core.Components;
using NopStation.Plugin.Widgets.OlarkChat.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Components;

public class OlarkChatViewComponent : NopStationViewComponent
{
    private readonly ISettingService _settingService;
    private readonly IWorkContext _workContext;
    private readonly ICustomerService _customerService;

    public OlarkChatViewComponent(
        ISettingService settingService,
        IWorkContext workContext,
        ICustomerService customerService)
    {
        _settingService = settingService;
        _workContext = workContext;
        _customerService = customerService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var settings = await _settingService.LoadSettingAsync<OlarkChatSettings>();
        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerName = await _customerService.GetCustomerFullNameAsync(customer);

        var model = new OlarkChatPublicInfoModel
        {
            SiteId = settings.SiteId,
            CustomerName = customerName,
            CustomerEmail = customer?.Email ?? "",
            WidgetPosition = settings.WidgetPosition,
            EnableMobile = settings.EnableMobile,
            UseDarkTheme = settings.UseDarkTheme,
            ConfigurationMode = settings.ConfigurationMode,
            CustomScript = settings.CustomScript
        };

        return View("OlarkChatPublicInfo", model);
    }
}
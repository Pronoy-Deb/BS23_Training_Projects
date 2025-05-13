using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.OlarkChat.Models;
using Nop.Services.Configuration;
using Nop.Services.Customers;

namespace Nop.Plugin.Widgets.OlarkChat.Components
{
    public class WidgetOlarkChatViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;

        public WidgetOlarkChatViewComponent(
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

            var model = new PublicInfoModel
            {
                SiteId = settings.SiteId,
                CustomerName = customerName,
                CustomerEmail = customer?.Email ?? "",
                WidgetPosition = settings.WidgetPosition,
                EnableMobile = settings.EnableMobile,
                UseDarkTheme = settings.UseDarkTheme
            };

            return View("PublicInfo", model);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Customers;
using NopStation.Plugin.Misc.Core.Components;
using NopStation.Plugin.Widgets.OlarkChat.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Components;

public class OlarkChatViewComponent : NopStationViewComponent
{
    private readonly IWorkContext _workContext;
    private readonly ICustomerService _customerService;
    private readonly OlarkChatSettings _olarkChatSettings;

    public OlarkChatViewComponent(IWorkContext workContext,
        ICustomerService customerService,
        OlarkChatSettings olarkChatSettings)
    {
        _workContext = workContext;
        _customerService = customerService;
        _olarkChatSettings = olarkChatSettings;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new PublicInfoModel();

        if (_olarkChatSettings.UseScriptMode)
        {
            model.CustomScript = _olarkChatSettings.CustomScript;
        }
        else
        {
            var olarkScript = @"
window.olark || (function (c) {
	var f = window, d = document, l = f.location.protocol == ""https:"" ? ""https:"" : ""http:"", z = c.name,
		r = ""load"";
	var nt = function () {
		f[z] = function () { (a.s = a.s || []).push(arguments) };
		var a = f[z]._ = {}, q = c.methods.length;
		while (q--) (function (n) {
			f[z][n] = function () {
				f[z](""call"", n, arguments)
			}
		})(c.methods[q]);
		a.l = c.loader;
		a.i = nt;
		a.p = { 0: +new Date };
		a.P = function (u) { a.p[u] = new Date - a.p[0] };
		function s() { a.P(r); f[z](r) }
		f.addEventListener ? f.addEventListener(r, s, false) : f.attachEvent(""on"" + r, s);
		var ld = function () {
			function p(hd) { hd = ""head""; return [""<"", hd, ""></"", hd, ""><body></body>""].join("""") }
			var b = d.createElement(""div""); b.innerHTML = p(); return b.getElementsByTagName(""head"")[0] || d.documentElement
		};
		var h = d.createElement(""script""), e = ld(); h.async = 1;
		h.src = l + ""//"" + a.l; e.appendChild(h)
	};
	nt()
})({
	loader: ""static.olark.com/jsclient/loader0.js"",
	name: ""olark"",
	methods: [""configure"", ""extend"", ""declare"", ""identify""]
});

";

            var customer = await _workContext.GetCurrentCustomerAsync();
            var customerName = await _customerService.GetCustomerFullNameAsync(customer);
            var customerEmail = customer.Email ?? "";

            if (!string.IsNullOrEmpty(customerEmail))
            {
                olarkScript += $@"
olark.configure('visitor', {{
	email: '{customerEmail}',
	nickname: '{customerName}'
}});
";
            }

            olarkScript += $@"
olark.configure('system', {{
	mobile: {_olarkChatSettings.EnableMobile.ToString().ToLower()}
}});

olark.configure('system.hb_dark_theme', {(_olarkChatSettings.UseDarkTheme ? "true" : "false")});

olark.configure('system.hb_position', '{_olarkChatSettings.WidgetPosition.ToLower()}');
";

            olarkScript += $@"

olark.identify(""{_olarkChatSettings.SiteId}"");
";
            model.CustomScript = olarkScript;
        }

        return View("~/Plugins/NopStation.Plugin.Widgets.OlarkChat/Views/PublicInfo.cshtml", model);
    }
}
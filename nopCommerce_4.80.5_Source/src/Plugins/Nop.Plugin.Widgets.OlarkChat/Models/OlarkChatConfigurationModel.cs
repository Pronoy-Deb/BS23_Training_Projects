using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Widgets.OlarkChat.Models;
public record OlarkChatConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.SiteId")]
    public string SiteId { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.WidgetPosition")]
    public string WidgetPosition { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.AvailableWidgetPositions")]
    public IList<string> AvailableWidgetPositions { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.EnableMobile")]
    public bool EnableMobile { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.UseDarkTheme")]
    public bool UseDarkTheme { get; set; }

    public string ConfigurationMode { get; set; }
    public List<SelectListItem> AvailableConfigurationModes { get; set; }
    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.CustomScript")]
    public string CustomScript { get; set; }
}
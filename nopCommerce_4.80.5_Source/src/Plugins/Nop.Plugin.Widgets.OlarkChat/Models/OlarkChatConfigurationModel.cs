using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NopStation.Plugin.Widgets.OlarkChat.Models;
public record OlarkChatConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.SiteId")]
    public string SiteId { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.WidgetPosition")]
    public string WidgetPosition { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.OlarkChat.Fields.AvailableWidgetPositions")]
    public IList<string> AvailableWidgetPositions { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.EnableMobile")]
    public bool EnableMobile { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.UseDarkTheme")]
    public bool UseDarkTheme { get; set; }

    public string ConfigurationMode { get; set; }
    public List<SelectListItem> AvailableConfigurationModes { get; set; }
    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.CustomScript")]
    public string CustomScript { get; set; }
}
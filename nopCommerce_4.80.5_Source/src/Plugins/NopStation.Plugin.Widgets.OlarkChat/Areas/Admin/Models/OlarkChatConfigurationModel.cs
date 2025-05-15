using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

public record OlarkChatConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.SiteId")]
    public string SiteId { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.WidgetPosition")]
    public string WidgetPosition { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.EnableMobile")]
    public bool EnableMobile { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.UseDarkTheme")]
    public bool UseDarkTheme { get; set; }

    public string ConfigurationMode { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Fields.CustomScript")]
    public string CustomScript { get; set; }
}
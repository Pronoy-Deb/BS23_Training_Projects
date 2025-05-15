using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

public record ConfigurationModel : BaseNopModel
{
    public ConfigurationModel()
    {
        AvailableWidgetPositions = [];
    }

    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.UseScriptMode")]
    public bool UseScriptMode { get; set; }
    public bool UseScriptMode_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.CustomScript")]
    public string CustomScript { get; set; }
    public bool CustomScript_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.SiteId")]
    public string SiteId { get; set; }
    public bool SiteId_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.WidgetPosition")]
    public string WidgetPosition { get; set; }
    public bool WidgetPosition_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.UseDarkTheme")]
    public bool UseDarkTheme { get; set; }
    public bool UseDarkTheme_OverrideForStore { get; set; }

    [NopResourceDisplayName("Admin.NopStation.OlarkChat.Configuration.Fields.EnableMobile")]
    public bool EnableMobile { get; set; }
    public bool EnableMobile_OverrideForStore { get; set; }

    public IList<SelectListItem> AvailableWidgetPositions { get; set; }
}
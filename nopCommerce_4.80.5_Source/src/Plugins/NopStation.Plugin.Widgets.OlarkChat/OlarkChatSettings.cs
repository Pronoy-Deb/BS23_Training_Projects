using Nop.Core.Configuration;

namespace NopStation.Plugin.Widgets.OlarkChat;

public class OlarkChatSettings : ISettings
{
    public string SiteId { get; set; }

    public bool UseScriptMode { get; set; }
    public string CustomScript { get; set; }

    public string WidgetPosition { get; set; }
    public bool UseDarkTheme { get; set; }
    public bool EnableMobile { get; set; }
}
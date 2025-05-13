using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.OlarkChat.Models
{
    public class OlarkChatSettings : ISettings
    {
        public string SiteId { get; set; }
        public string WidgetPosition { get; set; }
        public bool EnableMobile { get; set; }
        public bool UseDarkTheme { get; set; }
    }
}
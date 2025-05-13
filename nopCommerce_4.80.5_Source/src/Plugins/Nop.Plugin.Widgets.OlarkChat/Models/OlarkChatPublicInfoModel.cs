namespace Nop.Plugin.Widgets.OlarkChat.Models
{
    public record OlarkChatPublicInfoModel
    {
        public string SiteId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string WidgetPosition { get; set; }
        public bool EnableMobile { get; set; }
        public bool UseDarkTheme { get; set; }
    }
}
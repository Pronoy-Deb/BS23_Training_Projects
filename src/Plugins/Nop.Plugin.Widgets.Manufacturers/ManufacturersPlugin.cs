using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Plugin.Widgets.Manufacturers.Components;
namespace Nop.Plugin.Widgets.Manufacturers;

public class ManufacturersPlugin : BasePlugin, IWidgetPlugin
{
    public bool HideInWidgetList => false;

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { "home_page_before_news" });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ManufacturersViewComponent);
    }
}

namespace Nop.Plugin.Misc.ProductViewTracker;
using global::Nop.Plugin.Misc.ProductViewTracker.Components;
using global::Nop.Services.Cms;
using global::Nop.Services.Plugins;

using global::Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;

public class ProductViewTrackerPlugin : BasePlugin, IWidgetPlugin
{
    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => true;
    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductViewTrackerViewComponent);
    }
    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>Widget zones</returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsTop });
    }
}
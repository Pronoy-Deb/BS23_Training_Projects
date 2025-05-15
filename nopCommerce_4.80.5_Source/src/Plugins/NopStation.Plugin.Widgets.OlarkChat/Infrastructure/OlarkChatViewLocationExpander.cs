using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;

namespace NopStation.Plugin.Widgets.OlarkChat.Infrastructure;

public class OlarkChatViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        viewLocations = new string[]
        {
            $"/Plugins/NopStation.Plugin.Widgets.OlarkChat/Views/{{0}}.cshtml",
            $"/Plugins/NopStation.Plugin.Widgets.OlarkChat/Views/{{1}}/{{0}}.cshtml",
            $"/Plugins/NopStation.Plugin.Widgets.OlarkChat/Areas/Admin/Views/{{0}}.cshtml",
            $"/Plugins/NopStation.Plugin.Widgets.OlarkChat/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
        }.Concat(viewLocations);
        return viewLocations;
    }
}
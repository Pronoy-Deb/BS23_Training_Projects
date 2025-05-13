using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Widgets.OlarkChat.Infrastructure;

public class ViewLocationExpander : IViewLocationExpander
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
            $"/Plugins/Widgets.OlarkChat/Views/{{0}}.cshtml",
            $"/Plugins/Widgets.OlarkChat/Views/{{1}}/{{0}}.cshtml"
        }.Concat(viewLocations);
        return viewLocations;
    }
}
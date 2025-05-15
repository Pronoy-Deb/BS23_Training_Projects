using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;

namespace Nop.Plugin.Misc.ServiceSubscription.Infrastructure;

public class SSViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "admin")
        {
            viewLocations = new string[]
            {
                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Shared/{{0}}.cshtml",
                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Shared/{{1}}/{{0}}.cshtml",
                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Shared/Components/{{1}}/{{0}}.cshtml",

                $"/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Components/{{1}}/{{0}}.cshtml"

        }.Concat(viewLocations);
        }
        return viewLocations;
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Components;
//using Nop.Plugin.Misc.ServiceSubscription.Components;
using Nop.Services.Cms;

namespace Nop.Plugin.Misc.ServiceSubscription.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IWidgetPlugin, ServiceSubscriptionWidget>();
            services.AddTransient<ProductVariablePriceBlockViewComponent>();
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new SSViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public int Order => 3000;
    }
}

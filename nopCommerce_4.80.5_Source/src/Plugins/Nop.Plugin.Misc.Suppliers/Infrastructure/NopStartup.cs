//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Nop.Core.Infrastructure;
//using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
//using Nop.Plugin.Misc.Suppliers.Factories;
//using Nop.Plugin.Misc.Suppliers.Services;

//namespace Nop.Plugin.Misc.Suppliers.Infrastructure
//{
//    public class NopStartup : INopStartup
//    {
//        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//        {
//            services.AddScoped<ISuppliersService, SuppliersService>();
//            services.AddScoped<ISuppliersModelFactory, SuppliersModelFactory>();
//        }

//        public void Configure(IApplicationBuilder application) { }

//        public int Order => 3000;
//    }

//}


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Plugin.Misc.Suppliers.Factories;
using Nop.Plugin.Misc.Suppliers.Infrastructure;
using Nop.Plugin.Misc.Suppliers.Services;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISuppliersService, SuppliersService>();
            services.AddScoped<ISuppliersModelFactory, SuppliersModelFactory>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public int Order => 3000;
    }
}

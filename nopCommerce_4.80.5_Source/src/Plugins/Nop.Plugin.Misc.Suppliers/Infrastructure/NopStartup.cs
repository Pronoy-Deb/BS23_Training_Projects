using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.Suppliers.Services;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISuppliersService, SuppliersService>();
        }

        public void Configure(IApplicationBuilder application) { }

        public int Order => 3000;
    }
}
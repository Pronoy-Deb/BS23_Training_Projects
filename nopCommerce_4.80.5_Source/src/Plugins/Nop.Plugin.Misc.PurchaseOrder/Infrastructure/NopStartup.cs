using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.Suppliers.Infrastructure;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.ExportImport;

namespace Nop.Plugin.Misc.PurchaseOrder.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SuppliersService>();
            services.AddScoped<IPurchaseOrderSupplierService, PurchaseOrderSupplierService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IPurchaseOrderModelFactory, PurchaseOrderModelFactory>();
            services.AddScoped<IPurchaseOrderExportManager, PurchaseOrderExportManager>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new POViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public int Order => 3000;
    }
}

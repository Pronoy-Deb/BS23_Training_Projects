using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;

namespace NopStation.Plugin.Widgets.OlarkChat.Infrastructure;

public class PluginNopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IOlarkChatModelFactory, OlarkChatModelFactory>();
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new OlarkChatViewLocationExpander());
        });
    }

    public void Configure(IApplicationBuilder application)
    {
        
    }

    public int Order => 3000;
}
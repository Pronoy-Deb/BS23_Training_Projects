using NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Models;

namespace NopStation.Plugin.Widgets.OlarkChat.Areas.Admin.Factories;

public interface IOlarkChatModelFactory
{
    Task<OlarkChatConfigurationModel> PrepareConfigurationModelAsync();
    Task SaveConfigurationModelAsync(OlarkChatConfigurationModel model);
}

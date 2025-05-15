using Nop.Core.Domain.Customers;
using Nop.Services.Security;

namespace NopStation.Plugin.Widgets.OlarkChat;

public class OlarkChatPermissionProvider
{
    public const string MANAGE_CONFIGURATION = "OlarkChatManageConfig";
}

public class OlarkChatPermissionConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new("NopStation OlarkChat. Manage OlarkChat Configuration", OlarkChatPermissionProvider.MANAGE_CONFIGURATION, "NopStation", NopCustomerDefaults.AdministratorsRoleName)
    };
}

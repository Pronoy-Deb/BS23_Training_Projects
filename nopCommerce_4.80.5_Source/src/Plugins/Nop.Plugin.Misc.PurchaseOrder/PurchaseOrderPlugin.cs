using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.PurchaseOrder;
public class PurchaseOrderPlugin : BasePlugin
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;

    public PurchaseOrderPlugin(IPermissionService permissionService, ILocalizationService localizationService)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
    }

    private Dictionary<string, string> _resourceString = new Dictionary<string, string>
    {
        ["Admin.PurchaseOrder"] = "PurchaseOrder",
        ["plugins.misc.purchaseorder.purchaseorders"] = "Purchase Orders",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByStartDate"] = "Start Date",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByStartDate.Hint"] = "Search by Start Date",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByEndDate"] = "End Date",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByEndDate.Hint"] = "Search by End Date",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchBySupplierName"] = "Supplier Name",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchBySupplierName.Hint"] = "Search by Supplier Name",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByProductName"] = "Product Name",
        ["Plugins.Misc.PurchaseOrder.PurchaseOrderList.SearchByProductName.Hint"] = "Search by Product Name",

        ["Plugins.Misc.PurchaseOrder.Fields.OrderId"] = "Order #",
        ["Plugins.Misc.PurchaseOrder.Fields.SupplierName"] = "Supplier",
        ["Plugins.Misc.PurchaseOrder.Fields.OrderDate"] = "Created on",
        ["Plugins.Misc.PurchaseOrder.Fields.TotalAmount"] = "Order Total",
        ["Plugins.Misc.PurchaseOrder.Fields.CreatedBy"] = "Created By"

    };

    public bool HideInWidgetList => false;

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>(_resourceString));

        await base.UpdateAsync(currentVersion, targetVersion);
    }
}

public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    private readonly IPermissionService _permissionService;

    public EventConsumer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
            return;

        eventMessage.RootMenuItem.InsertAfter("Help",
            new AdminMenuItem
            {
                SystemName = "Misc.PurchaseOrder",
                Title = "PurchaseOrder",
                Url = eventMessage.GetMenuItemUrl("PurchaseOrder", "List"),
                IconClass = "far fa-dot-circle",
                Visible = true,
            });
    }
}
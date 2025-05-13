//using Nop.Plugin.Misc.ServiceSubscription.Components;
//using Nop.Services.Cms;
//using Nop.Services.Plugins;
//using Nop.Web.Framework.Infrastructure;

//namespace Nop.Plugin.Misc.ServiceSubscription.Widgets
//{
//    public class ServiceSubscriptionWidget : BasePlugin, IWidgetPlugin
//    {
//        public bool HideInWidgetList => false;

//        public Task<IList<string>> GetWidgetZonesAsync()
//        {
//            //return Task.FromResult<IList<string>>(new List<string> { AdminWidgetZones.ProductDetailsBlock });
//            return Task.FromResult<IList<string>>(new List<string>
//            {
//                "admin_product_details_block" // Try this first
//                //"Admin.Product.Details.Block", // Fallback for newer versions
//                //"product_details_block"        // Fallback for older versions
//            });
//        }

//        public Type GetWidgetViewComponent(string widgetZone)
//        {
//            return typeof(ProductVariablePriceBlockViewComponent);
//        }

//        public override async Task InstallAsync()
//        {
//            // Add any installation logic here
//            await base.InstallAsync();
//        }

//        public override async Task UninstallAsync()
//        {
//            // Add any uninstallation logic here
//            await base.UninstallAsync();
//        }
//    }
//}


////using Nop.Plugin.Misc.ServiceSubscription.Components;
////using Nop.Services.Cms;
////using Nop.Services.Plugins;
////using Nop.Web.Framework.Infrastructure;

////namespace Nop.Plugin.Misc.ServiceSubscription.Widgets;
////public class ServiceSubscriptionWidget : BasePlugin, IWidgetPlugin
////{
////    public bool HideInWidgetList => true;

////    public Task<IList<string>> GetWidgetZonesAsync()
////    {
////        return Task.FromResult<IList<string>>(new List<string> { "admin_product_details_block" });
////    }

////    public Type GetWidgetViewComponent(string widgetZone)
////    {
////        return typeof(ProductVariablePriceBlockViewComponent);
////    }
////}
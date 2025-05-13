using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class ServiceSubscriptionController : BasePluginController
    {
        //private readonly IProductController _productController;

        //public ServiceSubscriptionController(IProductController productController)
        //{
        //    _productController = productController;
        //}

        //public async Task<IActionResult> List()
        //{
        //    // Get the product list as you want it
        //    var productList = await _productController.List();

        //    // Create your custom model
        //    var model = new ServiceSubscriptionListModel
        //    {
        //        ProductListModel = productList
        //    };

        //    return View(model);
        //}
        public async Task<IActionResult> List()
        {
            return RedirectToAction("List", "Product", new { area = "Admin" });
        }

        //[HttpPost]
        //public async Task<IActionResult> List()
        //{
        //}

        //public async Task<IActionResult> Create()
        //{
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] PurchaseOrderCreateModel model)
        //{

        //}
    }
}
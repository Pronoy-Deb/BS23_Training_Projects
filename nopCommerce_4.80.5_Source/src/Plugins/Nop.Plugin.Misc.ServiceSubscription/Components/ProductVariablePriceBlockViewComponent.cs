using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Models;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Components
{
    [ViewComponent(Name = "ProductVariablePriceBlock")]
    public class ProductVariablePriceBlockViewComponent : NopViewComponent
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public ProductVariablePriceBlockViewComponent(
            IProductService productService,
            ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            // Extract productId from additionalData
            var productId = 0;
            if (additionalData is ProductModel productModel)
                productId = productModel.Id;


            //if (additionalData is int id)
            //{
            //    productId = id;
            //}
            //else if (additionalData is Dictionary<string, object> data && data.ContainsKey("productId"))
            //{
            //    productId = Convert.ToInt32(data["productId"]);
            //}

            // Fetch the product asynchronously
            var product = await _productService.GetProductByIdAsync(productId);

            // Map the Product entity to ProductModel
            productModel = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                // Map other necessary properties here
            };

            var model = new ProductVariablePriceBlockModel
            {
                ProductModel = productModel,
                AvailableCustomers = (await _customerService.GetAllCustomersAsync())
                    .Select(c => new SelectListItem
                    {
                        Text = c.Email,
                        Value = c.Id.ToString()
                    }).ToList(),
                HideBlockAttributeName = "ProductVariablePriceBlock.HideBlock",
                HideBlock = false
            };

            return View("~/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Shared/Components/ProductVariablePriceBlock/Default.cshtml", model);
        }
    }
}




//using Nop.Web.Areas.Admin.Models.Catalog;
//using Nop.Services.Customers;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc;
//using Nop.Core;
//using Nop.Services.Common;
//using Nop.Web.Framework.Components;
//using DocumentFormat.OpenXml.EMMA;

//namespace Nop.Plugin.Misc.ServiceSubscription.Components
//{
//    [ViewComponent(Name = "ProductVariablePriceBlock")]
//    public class ProductVariablePriceBlockViewComponent : NopViewComponent
//    {
//        private readonly IGenericAttributeService _genericAttributeService;
//        private readonly IWorkContext _workContext;
//        private readonly ICustomerService _customerService;

//        public ProductVariablePriceBlockViewComponent(
//            IGenericAttributeService genericAttributeService,
//            IWorkContext workContext,
//            ICustomerService customerService)
//        {
//            _genericAttributeService = genericAttributeService;
//            _workContext = workContext;
//            _customerService = customerService;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
//        {
//            const string hideBlockAttributeName = "ProductPage.HideVariablePricesBlock";
//            var customer = await _workContext.GetCurrentCustomerAsync();
//            var hideBlock = await _genericAttributeService.GetAttributeAsync<bool>(customer, hideBlockAttributeName);

//            // Get product model from additional data
//            var productModel = additionalData as ProductModel;

//            // Get all customers for dropdown
//            var customers = await _customerService.GetAllCustomersAsync();
//            return View("~/Plugins/Misc.ServiceSubscription/Areas/Admin/Views/Shared/Components/ProductVariablePriceBlock/Default.cshtml");
//            //return View(new
//            //{
//            //    HideBlock = hideBlock,
//            //    HideBlockAttributeName = hideBlockAttributeName,
//            //    ProductModel = productModel,
//            //    AvailableCustomers = customers.Select(c => new SelectListItem
//            //    {
//            //        Text = c.Email,
//            //        Value = c.Id.ToString()
//            //    }).ToList()
//            //});
//        }
//    }
//}
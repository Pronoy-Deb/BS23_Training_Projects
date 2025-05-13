using Microsoft.AspNetCore.Mvc;
using Nop.Data;
using Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Models;
using Nop.Plugin.Misc.ServiceSubscription.Domain;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.ServiceSubscription.Areas.Admin.Controllers
{
    [AutoValidateAntiforgeryToken]
    [AuthorizeAdmin]
    [Area("Admin")]
    public class VariablePriceController : BasePluginController
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IRepository<VariablePrice> _variablePriceRepository;

        public VariablePriceController(
            IProductService productService,
            ICustomerService customerService,
            IRepository<VariablePrice> variablePriceRepository)
        {
            _productService = productService;
            _customerService = customerService;
            _variablePriceRepository = variablePriceRepository;
        }

        [HttpPost]
        public IActionResult AddVariablePrice(int productId, int customerId, decimal price)
        {
            var existing = _variablePriceRepository.Table
                .FirstOrDefault(v => v.ProductId == productId && v.CustomerId == customerId);

            if (existing != null)
            {
                existing.Price = price;
                _variablePriceRepository.Update(existing);
            }
            else
            {
                _variablePriceRepository.Insert(new VariablePrice
                {
                    ProductId = productId,
                    CustomerId = customerId,
                    Price = price
                });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteVariablePrice(int id)
        {
            var variablePrice = _variablePriceRepository.GetById(id);
            if (variablePrice != null)
            {
                _variablePriceRepository.Delete(variablePrice);
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> VariablePriceList(int productId)
        {
            var variablePrices = _variablePriceRepository.Table
                .Where(v => v.ProductId == productId)
                .ToList();

            var model = new VariablePriceListModel
            {
                Data = new List<VariablePriceModel>()
            };

            foreach (var v in variablePrices)
            {
                var customer = await _customerService.GetCustomerByIdAsync(v.CustomerId);
                model.Data.Add(new VariablePriceModel
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerEmail = customer?.Email,
                    Price = v.Price,
                    ProductId = v.ProductId
                });
            }

            return Json(model);
        }


        //public IActionResult VariablePriceList(int productId)
        //{
        //    var variablePrices = _variablePriceRepository.Table
        //        .Where(v => v.ProductId == productId)
        //        .ToList();

        //    var model = new VariablePriceListModel
        //    {
        //        Data = variablePrices.Select(v => new VariablePriceModel
        //        {
        //            Id = v.Id,
        //            CustomerId = v.CustomerId,
        //            CustomerEmail = _customerService.GetCustomerByIdAsync(v.CustomerId)?.Email,
        //            Price = v.Price,
        //            ProductId = v.ProductId
        //        }).ToList()
        //    };

        //    return Json(model);
        //}
    }


    //[AutoValidateAntiforgeryToken]
    //[AuthorizeAdmin]
    //[Area("admin")]

    //public class VariablePriceController : BasePluginController
    //{
    //    private readonly IProductService _productService;
    //    private readonly ICustomerService _customerService;

    //    public VariablePriceController(
    //        IProductService productService,
    //        ICustomerService customerService)
    //    {
    //        _productService = productService;
    //        _customerService = customerService;
    //    }

    //    [HttpPost]
    //    public IActionResult AddVariablePrice(int productId, int customerId, decimal price)
    //    {
    //        // Implement logic to save variable price
    //        // You'll need to create a new database table for this

    //        return Json(new { Result = true });
    //    }

    //    [HttpPost]
    //    public IActionResult DeleteVariablePrice(int id)
    //    {
    //        // Implement logic to delete variable price

    //        return Json(new { Result = true });
    //    }

    //    public IActionResult VariablePriceList(int productId)
    //    {
    //        // Implement logic to get variable prices for the product

    //        var model = new VariablePriceListModel();
    //        // Populate model with data

    //        return Json(model);
    //    }
    //}
}
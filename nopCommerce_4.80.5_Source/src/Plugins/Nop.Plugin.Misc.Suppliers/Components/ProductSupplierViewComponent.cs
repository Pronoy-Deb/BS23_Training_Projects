//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc;
//using Nop.Plugin.Misc.Suppliers.Services;
//using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
//using Nop.Web.Framework.Components;

//namespace Nop.Plugin.Misc.Suppliers.Components;
//[ViewComponent(Name = "ProductSupplierSelection")]
//public class ProductSupplierViewComponent : NopViewComponent
//{
//    private readonly ISuppliersService _suppliersService;
//    private readonly IProductSupplierService _productSupplierService;

//    public ProductSupplierViewComponent(
//        ISuppliersService suppliersService,
//        IProductSupplierService productSupplierService)
//    {
//        _suppliersService = suppliersService;
//        _productSupplierService = productSupplierService;
//    }

//    public async Task<IViewComponentResult> InvokeAsync(int productId)
//    {
//        var model = new ProductSupplierModel
//        {
//            ProductId = productId
//        };

//        var allSuppliers = await _suppliersService.GetAllSuppliersAsync();
//        var selectedSuppliers = await _productSupplierService.GetProductSuppliersByProductIdAsync(productId);

//        model.AvailableSuppliers = allSuppliers.Select(s => new SelectListItem
//        {
//            Text = s.Name,
//            Value = s.Id.ToString(),
//            Selected = selectedSuppliers.Any(ps => ps.SupplierId == s.Id)
//        }).ToList();

//        return View("ProductSupplier", model);
//    }
//}
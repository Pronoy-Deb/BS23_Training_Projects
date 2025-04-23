using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.Suppliers.Components;
public class ProductSupplierViewComponent : ViewComponent
{
    private readonly ISuppliersService _supplierService;
    private readonly IProductService _productService;
    private readonly IProductSupplierService _productSupplierService;

    public ProductSupplierViewComponent(
        ISuppliersService supplierService,
        IProductService productService,
        IProductSupplierService productSupplierService)
    {
        _supplierService = supplierService;
        _productService = productService;
        _productSupplierService = productSupplierService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        int productId = 0;

        if (additionalData is ProductModel productModel)
        {
            productId = productModel.Id;
        }

        var product = await _productService.GetProductByIdAsync(productId);

        var allSuppliers = await _supplierService.GetAllAsync();
        var assignedSupplierIds = await _productSupplierService.GetSupplierIdsByProductIdAsync(product.Id);

        var assignedSuppliers = allSuppliers
            .Where(s => assignedSupplierIds.Contains(s.Id))
            .Select(s => new AssignedSupplierModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email
            }).ToList();

        var availableSuppliers = allSuppliers
            .Where(s => !assignedSupplierIds.Contains(s.Id))
            .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

        var viewModel = new ProductSupplierModel
        {
            ProductId = product?.Id ?? 0,
            SelectedSupplierId = 0,
            AvailableSuppliers = availableSuppliers,
            AssignedSuppliers = assignedSuppliers
        };
        return View(viewModel);
    }
}

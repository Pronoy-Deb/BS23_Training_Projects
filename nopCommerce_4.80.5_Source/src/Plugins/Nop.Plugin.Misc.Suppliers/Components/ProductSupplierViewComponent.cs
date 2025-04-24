using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.Suppliers.Components
{
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
                productId = productModel.Id;

            if (productId <= 0)
            {
                return View(new ProductSupplierModel
                {
                    ProductId = 0,
                    AvailableSuppliers = new List<SelectListItem>(),
                    AssignedSupplier = null
                });
            }

            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return View(new ProductSupplierModel
                {
                    ProductId = productId,
                    AvailableSuppliers = new List<SelectListItem>(),
                    AssignedSupplier = null
                });
            }

            var allSuppliers = await _supplierService.GetAllAsync();
            var assignedSupplierIds = await _productSupplierService.GetSupplierIdsByProductIdAsync(product.Id);
            var assignedSupplierId = assignedSupplierIds.FirstOrDefault();

            var assignedSupplier = allSuppliers
                .Where(s => s.Id == assignedSupplierId)
                .Select(s => new AssignedSupplierModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Active = s.Active
                })
                .FirstOrDefault();

            var availableSuppliers = allSuppliers
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                    Selected = s.Id == assignedSupplierId
                }).ToList();

            var viewModel = new ProductSupplierModel
            {
                ProductId = product.Id,
                AvailableSuppliers = availableSuppliers,
                AssignedSupplier = assignedSupplier
            };

            return View(viewModel);
        }
    }
}
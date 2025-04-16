using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.Suppliers.Models;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.Suppliers.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]

    public class SuppliersController : BasePluginController
    {
        private readonly ISuppliersService _supplierService;
        private readonly IPermissionService _permissionService;

        public SuppliersController(ISuppliersService supplierService, IPermissionService permissionService)
        {
            _supplierService = supplierService;
            _permissionService = permissionService;
        }

        public IActionResult List()
        {
            var model = new SuppliersSearchModel();
            model.SetGridPageSize();
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/List.cshtml", model);
        }


        [HttpPost]
        public async Task<IActionResult> List(SuppliersSearchModel searchModel)
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync(
                searchModel.SearchName,
                searchModel.SearchEmail,
                searchModel.Active,
                searchModel.Page - 1,
                searchModel.PageSize
                );

            var model = await new SuppliersListModel().PrepareToGridAsync(searchModel, suppliers, () =>
            {
                return suppliers.SelectAwait(async supplier =>
                {
                    var supplierModel = new SuppliersModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name ?? string.Empty,
                        Email = supplier.Email ?? string.Empty,
                        IsActive = supplier.IsActive
                    };

                    return supplierModel;
                });
            });

            return Json(model);
        }

        public IActionResult Create()
        {
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(SuppliersRecord supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.InsertAsync(supplier);
                return RedirectToAction("Index");
            }
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/Create.cshtml", supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/Edit.cshtml", supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SuppliersRecord supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.UpdateAsync(supplier);
                return RedirectToAction("Index");
            }
            return View("~/Plugins/Misc.Suppliers/Areas/Admin/Views/Edit.cshtml", supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier != null)
                await _supplierService.DeleteAsync(supplier);

            return RedirectToAction("Index");
        }
    }
}
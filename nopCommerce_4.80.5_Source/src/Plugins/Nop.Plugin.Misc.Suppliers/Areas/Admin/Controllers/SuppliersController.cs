using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]

    public class SuppliersController : BasePluginController
    {
        private readonly ISuppliersService _supplierService;
        private readonly IPermissionService _permissionService;
        private readonly ISuppliersModelFactory _suppliersModelFactory;

        public SuppliersController(ISuppliersService supplierService,
            IPermissionService permissionService,
            ISuppliersModelFactory suppliersModelFactory)
        {
            _supplierService = supplierService;
            _permissionService = permissionService;
            _suppliersModelFactory = suppliersModelFactory;
        }

        public IActionResult List()
        {
            var model = new SuppliersSearchModel();
            model.SetGridPageSize();
            return View("List", model);
        }


        [HttpPost]
        public async Task<IActionResult> List(SuppliersSearchModel searchModel)
        {
            var model = await _suppliersModelFactory.PrepareSuppliersListModelAsync(searchModel);
            return Json(model);
        }


        public async Task<IActionResult> Create()
        {
            var model = await _suppliersModelFactory.PrepareSuppliersModelAsync(new SuppliersModel(), null);
            return View("Create", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SuppliersModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var entity = new SuppliersRecord
                {
                    Name = model.Name,
                    Email = model.Email,
                    IsActive = model.Active
                };

                await _supplierService.InsertAsync(entity);

                if (continueEditing)
                    return RedirectToAction("Edit", new { id = entity.Id });

                return RedirectToAction("List");
            }

            model = await _suppliersModelFactory.PrepareSuppliersModelAsync(model, null);
            return View("Create", model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            return View("Edit", supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SuppliersRecord supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierService.UpdateAsync(supplier);
                return RedirectToAction("List");
            }

            return View("Edit", supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier != null)
                await _supplierService.DeleteAsync(supplier);

            return RedirectToAction("List");
        }
    }
}
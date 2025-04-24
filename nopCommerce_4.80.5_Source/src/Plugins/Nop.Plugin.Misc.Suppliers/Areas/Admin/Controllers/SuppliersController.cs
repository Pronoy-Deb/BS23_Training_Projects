using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Plugin.Misc.Suppliers.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]

    public class SuppliersController : BasePluginController
    {

        private readonly ISuppliersService _supplierService;
        private readonly IProductSupplierService _productSupplierService;
        private readonly ISuppliersModelFactory _suppliersModelFactory;
        private readonly ILocalizedEntityService _localizedEntityService;

        public SuppliersController(
            ISuppliersService supplierService,
            ISuppliersModelFactory suppliersModelFactory,
            ILocalizedEntityService localizedEntityService,
            IProductSupplierService productSupplierService)
        {
            _supplierService = supplierService;
            _suppliersModelFactory = suppliersModelFactory;
            _localizedEntityService = localizedEntityService;
            _productSupplierService = productSupplierService;
        }

        public IActionResult List()
        {
            var model = new SuppliersSearchModel();
            model.SetGridPageSize();
            return View(model);
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
            return View(model);
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
                    Description = model.Description,
                    PictureId = model.PictureId,
                    AdminComment = model.AdminComment,
                    Active = model.Active
                };

                await _supplierService.InsertAsync(entity);

                await UpdateLocales(entity, model);

                if (continueEditing)
                    return RedirectToAction("Edit", new { id = entity.Id });

                return RedirectToAction("List");
            }

            model = await _suppliersModelFactory.PrepareSuppliersModelAsync(model, null);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
                return RedirectToAction("List");

            var model = await _suppliersModelFactory.PrepareSuppliersModelAsync(null, supplier);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SuppliersModel model)
        {
            var entity = await _supplierService.GetByIdAsync(model.Id);
            if (entity == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity.Name = model.Name;
                entity.Email = model.Email;
                entity.Description = model.Description;
                entity.PictureId = model.PictureId;
                entity.AdminComment = model.AdminComment;
                entity.Active = model.Active;

                await _supplierService.UpdateAsync(entity);

                await UpdateLocales(entity, model);

                return RedirectToAction("List");
            }

            model = await _suppliersModelFactory.PrepareSuppliersModelAsync(model, entity);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier != null)
                await _supplierService.DeleteAsync(supplier);

            return RedirectToAction("List");
        }
        protected virtual async Task UpdateLocales(SuppliersRecord supplier, SuppliersModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignSupplier(int productId, int supplierId)
        {
            if (productId == 0 || supplierId == 0)
                return BadRequest("Invalid product or supplier ID");

            await _productSupplierService.UpdateProductSupplierAsync(productId, supplierId);

            return Json(new { success = true });
        }
    }
}
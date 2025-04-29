using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Plugin.Misc.PurchaseOrder.ExportImport;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Controllers
{
    [Area(AreaNames.ADMIN)]
    public class PurchaseOrderController : BasePluginController
    {
        private readonly IPurchaseOrderModelFactory _purchaseOrderModelFactory;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderExportManager _exportManager;

        public PurchaseOrderController(IPurchaseOrderModelFactory purchaseOrderModelFactory,
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderExportManager exportManager)
        {
            _purchaseOrderModelFactory = purchaseOrderModelFactory;
            _purchaseOrderService = purchaseOrderService;
            _exportManager = exportManager;
        }

        public async Task<IActionResult> List()
        {
            var searchModel = new PurchaseOrderSearchModel();
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderSearchModelAsync(searchModel);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(PurchaseOrderSearchModel searchModel)
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderListModelAsync(searchModel);
            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(new PurchaseOrderModel(), null);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseOrderModel model)
        {
            if (!ModelState.IsValid)
                return await Create();

            await _purchaseOrderModelFactory.InsertPurchaseOrderAsync(model);

            return RedirectToAction("List");
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var purchaseOrder = await _purchaseOrderModelFactory.GetPurchaseOrderByIdAsync(id);
        //    if (purchaseOrder == null)
        //        return RedirectToAction("List");

        //    var model = await _purchaseOrderModelFactory.PreparePurchaseOrderModelAsync(null, purchaseOrder);
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(PurchaseOrderModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return await Edit(model.Id);

        //    await _purchaseOrderModelFactory.UpdatePurchaseOrderAsync(model);

        //    return RedirectToAction("List");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _purchaseOrderModelFactory.DeletePurchaseOrderAsync(id);
        //    return RedirectToAction("List");
        //}

        [HttpPost]
        public virtual async Task<IActionResult> ExportCsv(PurchaseOrderSearchModel searchModel)
        {
            // Await the filtered orders
            var pagedOrders = await _purchaseOrderService.GetFilteredOrders(searchModel);

            // Convert to list for export
            var bytes = _exportManager.ExportPurchaseOrdersToCsv(pagedOrders.ToList());

            return File(bytes, MimeTypes.TextCsv, "purchaseorders.csv");
        }

    }
}

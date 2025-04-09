using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Services.Plugins;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Manufacturers.Components;

public class ManufacturersViewComponent : NopViewComponent

{

    private readonly IManufacturerService _manufacturerService;

    private readonly IPictureService _pictureService;

    public ManufacturersViewComponent(IManufacturerService manufacturerService, IPictureService pictureService)

    {
        _manufacturerService = manufacturerService;

        _pictureService = pictureService;

    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)

    {

        var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: false);

        var images = new List<string>();

        foreach (var m in manufacturers)

        {

            var imageUrl = await _pictureService.GetPictureUrlAsync(m.PictureId);

            images.Add(imageUrl ?? "/images/default.png");
        }

        return View("~/Plugins/Widgets.Manufacturers/Views/ShowManufacturers.cshtml", images);

    }

}


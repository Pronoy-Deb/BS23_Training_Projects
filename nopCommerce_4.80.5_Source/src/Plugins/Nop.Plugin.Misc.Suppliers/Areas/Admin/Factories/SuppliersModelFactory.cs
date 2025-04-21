using System.Linq;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.Suppliers.Factories
{
    public class SuppliersModelFactory : ISuppliersModelFactory
    {

        private readonly ISuppliersService _supplierService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILanguageService _languageService;

        public SuppliersModelFactory(
            ISuppliersService supplierService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILanguageService languageService)
        {
            _supplierService = supplierService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _languageService = languageService;
        }

        public async Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel)
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
                    return new SuppliersModel
                    {
                        Id = supplier.Id,
                        Name = supplier.Name ?? string.Empty,
                        Email = supplier.Email ?? string.Empty,
                        Description = supplier.Description ?? string.Empty,
                        PictureId = supplier.PictureId,
                        AdminComment = supplier.AdminComment ?? string.Empty,
                        Active = supplier.Active
                    };
                });
            });

            return model;
        }

        public async Task<SuppliersModel> PrepareSuppliersModelAsync(SuppliersModel model, SuppliersRecord supplier)
        {
            if (supplier != null)
            {
                if (model == null)
                {
                    model = new SuppliersModel
                    {
                        Id = supplier.Id,
                        Name = await _localizationService.GetLocalizedAsync(supplier, x => x.Name),
                        Email = supplier.Email,
                        Description = await _localizationService.GetLocalizedAsync(supplier, x => x.Description),
                        PictureId = supplier.PictureId,
                        AdminComment = supplier.AdminComment,
                        Active = supplier.Active
                    };
                }
                var languages = await _languageService.GetAllLanguagesAsync(true);
                model.Locales = await languages.SelectAwait(async language => new SuppliersLocalizedModel
                {
                    LanguageId = language.Id,
                    Name = await _localizationService.GetLocalizedAsync(supplier, x => x.Name, language.Id, false),
                    Description = await _localizationService.GetLocalizedAsync(supplier, x => x.Description, language.Id, false)
                }).ToListAsync();
            }
            else
            {
                model ??= new SuppliersModel();
                var languages = await _languageService.GetAllLanguagesAsync(true);
                model.Locales = languages.Select(language => new SuppliersLocalizedModel
                {
                    LanguageId = language.Id
                }).ToList();
            }

            return model;
        }


        public async Task<SuppliersSearchModel> PrepareSuppliersSearchModelAsync(SuppliersSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            await Task.CompletedTask;

            searchModel.SetGridPageSize();

            return searchModel;
        }
    }
}

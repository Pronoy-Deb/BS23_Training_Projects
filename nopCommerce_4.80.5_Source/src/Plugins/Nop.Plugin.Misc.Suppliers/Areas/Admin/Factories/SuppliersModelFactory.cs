using System.Linq;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Factories
{
    public class SuppliersModelFactory : ISuppliersModelFactory
    {
        private readonly ISuppliersService _supplierService;

        public SuppliersModelFactory(ISuppliersService supplierService)
        {
            _supplierService = supplierService;
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
                    model = new SuppliersModel()
                    {
                        Id = supplier.Id,
                        Name = supplier.Name,
                        Email = supplier.Email,
                        Description = supplier.Description,
                        PictureId = supplier.PictureId,
                        AdminComment = supplier.AdminComment,
                        Active = supplier.Active
                    };
                }
            }
            await Task.CompletedTask;

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

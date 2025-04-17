using System.Linq;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Services;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;

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
                        IsActive = supplier.IsActive
                    };
                });
            });

            return model;
        }
    }
}

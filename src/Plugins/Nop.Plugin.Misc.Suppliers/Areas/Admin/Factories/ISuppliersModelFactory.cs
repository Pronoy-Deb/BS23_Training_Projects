using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;

public interface ISuppliersModelFactory
{
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);
    Task<SuppliersModel> PrepareSuppliersModelAsync(SuppliersModel model, SuppliersRecord supplier);
    Task<SuppliersSearchModel> PrepareSuppliersSearchModelAsync(SuppliersSearchModel searchModel);
}

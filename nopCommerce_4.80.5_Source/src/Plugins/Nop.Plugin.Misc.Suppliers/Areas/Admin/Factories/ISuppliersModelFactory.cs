using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Factories;

public interface ISuppliersModelFactory
{
    Task<SuppliersListModel> PrepareSuppliersListModelAsync(SuppliersSearchModel searchModel);
}

using Nop.Core;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services
{
    public interface ISuppliersService
    {
        Task InsertAsync(SuppliersRecord supplier);
        Task UpdateAsync(SuppliersRecord supplier);
        Task DeleteAsync(SuppliersRecord supplier);
        Task<SuppliersRecord> GetByIdAsync(int id);
        Task<IList<SuppliersRecord>> GetAllAsync();

        Task<IPagedList<SuppliersRecord>> GetPagedSuppliersAsync(string searchTerm, int pageIndex,
            int pageSize);

        Task<IPagedList<SuppliersRecord>> GetAllSuppliersAsync(
            string name = null,
            string email = null,
            bool? active = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);
    }
}
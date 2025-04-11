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
    }
}
using Nop.Plugin.Misc.Suppliers.Domain;

using Nop.Data;

namespace Nop.Plugin.Misc.Suppliers.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IRepository<SuppliersRecord> _repository;

        public SuppliersService(IRepository<SuppliersRecord> repository)
        {
            _repository = repository;
        }

        public async Task InsertAsync(SuppliersRecord supplier) => await _repository.InsertAsync(supplier);
        public async Task UpdateAsync(SuppliersRecord supplier) => await _repository.UpdateAsync(supplier);
        public async Task DeleteAsync(SuppliersRecord supplier) => await _repository.DeleteAsync(supplier);
        public async Task<SuppliersRecord> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<IList<SuppliersRecord>> GetAllAsync() => await _repository.GetAllAsync(
        (Func<IQueryable<SuppliersRecord>, IQueryable<SuppliersRecord>>)(query => query));
    }
}

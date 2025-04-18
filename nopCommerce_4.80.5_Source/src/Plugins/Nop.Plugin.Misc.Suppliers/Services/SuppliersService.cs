using Nop.Plugin.Misc.Suppliers.Domain;

using Nop.Data;
using Nop.Core;

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
        public async Task<IPagedList<SuppliersRecord>> GetAllSuppliersAsync(
            string name = null, 
            string email = null, 
            bool? active = null,
            int pageIndex = 0, 
            int pageSize = int.MaxValue)
        {
            var query = _repository.Table;
            
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));
            
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(s => s.Email.Contains(email));
            
            if (active.HasValue)
                query = query.Where(s => s.Active == active.Value);
            
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public Task<IList<SuppliersRecord>> GetAllAsync(string name = null, string email = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }
    }
}

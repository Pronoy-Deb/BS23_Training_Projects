using Nop.Plugin.Misc.Suppliers.Domain;
using Nop.Data;
using Nop.Core;
using Nop.Core.Caching;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.Suppliers.Services
{
    public class SuppliersService : ISuppliersService
    {
        private readonly IRepository<SuppliersRecord> _repository;
        private readonly IStaticCacheManager _staticCacheManager;

        public SuppliersService(IRepository<SuppliersRecord> repository, IStaticCacheManager staticCacheManager)
        {
            _repository = repository;
            _staticCacheManager = staticCacheManager;
        }

        public async Task InsertAsync(SuppliersRecord supplier)
        {
            await _repository.InsertAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SuppliersDefaults.AdminSupplierAllPrefixCacheKey);
        }

        public async Task UpdateAsync(SuppliersRecord supplier)
        {
            await _repository.UpdateAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SuppliersDefaults.AdminSupplierAllPrefixCacheKey);
            await _staticCacheManager.RemoveAsync(_staticCacheManager.PrepareKeyForDefaultCache(SuppliersDefaults.SupplierByIdCacheKey, supplier.Id));
        }

        public async Task DeleteAsync(SuppliersRecord supplier)
        {
            await _repository.DeleteAsync(supplier);
            await _staticCacheManager.RemoveByPrefixAsync(SuppliersDefaults.AdminSupplierAllPrefixCacheKey);
            await _staticCacheManager.RemoveAsync(_staticCacheManager.PrepareKeyForDefaultCache(SuppliersDefaults.SupplierByIdCacheKey, supplier.Id));
        }

        public async Task<SuppliersRecord> GetByIdAsync(int id)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(SuppliersDefaults.SupplierByIdCacheKey, id);
            return await _staticCacheManager.GetAsync(cacheKey, async () => await _repository.GetByIdAsync(id));
        }

        public async Task<IPagedList<SuppliersRecord>> GetAllSuppliersAsync(string name = null, string email = null, bool? active = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(SuppliersDefaults.AdminSupplierAllModelKey, name, email, active?.ToString() ?? "null", pageIndex, pageSize);

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var query = _repository.Table;

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(s => s.Name.Contains(name));
                if (!string.IsNullOrWhiteSpace(email))
                    query = query.Where(s => s.Email.Contains(email));
                if (active.HasValue)
                    query = query.Where(s => s.Active == active.Value);

                return await query.ToPagedListAsync(pageIndex, pageSize);
            });
        }

        public async Task<IList<SuppliersRecord>> GetSuppliersByIdsAsync(IList<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<SuppliersRecord>();

            return await _repository.Table
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public virtual async Task<IList<SuppliersRecord>> GetAllAsync()
        {
            return await _repository.GetAllAsync(query => query.OrderBy(s => s.Name));
        }
        public async Task<List<SelectListItem>> GetAllSuppliersForDropdownAsync()
        {
            var suppliers = await GetAllAsync();

            var items = suppliers.Select(s => new SelectListItem
            {
                Text = s.Name, // Or $"{s.Name} ({s.Email})" if you want more detail
                Value = s.Id.ToString()
            }).ToList();

            items.Insert(0, new SelectListItem { Text = "Select a supplier", Value = "0" });

            return items;
        }

    }
}
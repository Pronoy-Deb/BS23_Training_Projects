using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.PurchaseOrder.Services
{
    public partial class PurchaseOrderSupplierService : IPurchaseOrderSupplierService
    {
        private readonly IRepository<SuppliersRecord> _supplierRepository;

        // Constructor with proper injection of the repository
        public PurchaseOrderSupplierService(IRepository<SuppliersRecord> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        // Correct method definition
        public async Task<List<SuppliersRecord>> GetAllSuppliersAsync()
        {
            var suppliers = _supplierRepository.Table.ToList();
            return suppliers;
        }
    }
}
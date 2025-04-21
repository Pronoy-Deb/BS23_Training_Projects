using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Misc.Suppliers.Domain;

namespace Nop.Plugin.Misc.Suppliers.Services;
public interface IProductSupplierService
{
    Task InsertProductSupplierAsync(ProductSupplier productSupplier);
    Task UpdateProductSupplierAsync(ProductSupplier productSupplier);
    Task DeleteProductSupplierAsync(ProductSupplier productSupplier);
    Task<IList<ProductSupplier>> GetProductSuppliersByProductIdAsync(int productId);
    Task<IList<ProductSupplier>> GetProductSuppliersBySupplierIdAsync(int supplierId);
}
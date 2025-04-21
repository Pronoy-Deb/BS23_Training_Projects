using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Misc.Suppliers.Domain;
public class ProductSupplier : BaseEntity
{
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public int DisplayOrder { get; set; }
}
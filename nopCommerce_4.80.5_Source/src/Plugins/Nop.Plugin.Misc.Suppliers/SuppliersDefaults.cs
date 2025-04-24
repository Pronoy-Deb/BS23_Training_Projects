using Nop.Core.Caching;

public static class SuppliersDefaults
{
    public static CacheKey AdminSupplierAllModelKey => new("Nop.supplier.admin.model-{0}-{1}-{2}-{3}", AdminSupplierAllPrefixCacheKey);
    public static string AdminSupplierAllPrefixCacheKey => "Nop.supplier.admin.model";

    public static CacheKey SupplierByIdCacheKey => new("Nop.supplier.byid.{0}", SupplierByIdPrefixCacheKey);
    public static string SupplierByIdPrefixCacheKey => "Nop.supplier.byid";

    public static CacheKey ProductSuppliersByProductIdCacheKey => new("Nop.productsupplier.byproductid.{0}", ProductSuppliersPrefixCacheKey);
    public static string ProductSuppliersPrefixCacheKey => "Nop.productsupplier.byproductid";
}




//using Nop.Core.Caching;

//namespace Nop.Plugin.Misc.Suppliers;

//public static class SuppliersDefaults
//{
//    //public static CacheKey AdminSupplierAllModelKey => new("Nop.suppliers.admin.model-{0}-{1}-{2}-{3}", AdminSupplierAllPrefixCacheKey);
//    //public static string AdminSupplierAllPrefixCacheKey => "Nop.suppliers.admin.model";

//    // New cache keys for product-supplier mapping
//    public static CacheKey ProductSuppliersByProductIdCacheKey => new("Nop.suppliers.product.suppliers-{0}", ProductSuppliersPrefixCacheKey);
//    public static string ProductSuppliersPrefixCacheKey => "Nop.suppliers.product.suppliers-{0}";
//}




//using Nop.Core.Caching;

//namespace Nop.Plugin.Misc.Suppliers;
//public static class SuppliersDefaults
//{
//    public static CacheKey AdminSupplierAllModelKey => new("Nop.supplier.admin.model-{0}-{1}-{2}-{3}", AdminSupplierAllPrefixCacheKey);
//    public static string AdminSupplierAllPrefixCacheKey => "Nop.supplier.admin.model";
//}
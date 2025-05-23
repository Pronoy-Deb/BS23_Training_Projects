﻿using Nop.Core.Caching;

public static class SuppliersDefaults
{
    public static CacheKey AdminSupplierAllModelKey => new("Nop.supplier.admin.model-{0}-{1}-{2}-{3}", AdminSupplierAllPrefixCacheKey);
    public static string AdminSupplierAllPrefixCacheKey => "Nop.supplier.admin.model";

    public static CacheKey SupplierByIdCacheKey => new("Nop.supplier.byid.{0}", SupplierByIdPrefixCacheKey);
    public static string SupplierByIdPrefixCacheKey => "Nop.supplier.byid";

    public static CacheKey ProductSuppliersByProductIdCacheKey => new("Nop.productsupplier.byproductid.{0}", ProductSuppliersPrefixCacheKey);
    public static string ProductSuppliersPrefixCacheKey => "Nop.productsupplier.byproductid";
}

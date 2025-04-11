using Nop.Plugin.Misc.ProductViewTracker.Domain;

namespace Nop.Plugin.Misc.ProductViewTracker.Services;

using Nop.Data;
using System;

public interface IProductViewTrackerService
{
    /// <summary>
    /// Logs the specified record.
    /// </summary>
    /// <param name="record">The record.</param>
    void Log(ProductViewTrackerRecord record);
}

public class ProductViewTrackerService : IProductViewTrackerService
{
    private readonly IRepository<ProductViewTrackerRecord> _productViewTrackerRecordRepository;
    public ProductViewTrackerService(IRepository<ProductViewTrackerRecord> productViewTrackerRecordRepository)
    {
        _productViewTrackerRecordRepository = productViewTrackerRecordRepository;
    }
    /// <summary>
    /// Logs the specified record.
    /// </summary>
    /// <param name="record">The record.</param>
    public virtual void Log(ProductViewTrackerRecord record)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));
        _productViewTrackerRecordRepository.Insert(record);
    }
}

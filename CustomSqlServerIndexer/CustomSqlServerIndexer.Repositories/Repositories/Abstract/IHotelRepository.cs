using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Repositories;

public interface IHotelRepository : IRepositoryPrimaryKeyBase<Hotel, int>
{
    /// <summary>
    /// Gets the latest changes based on the row version number in ASCENDING order.
    /// </summary>
    /// <param name="highWaterMarkRowVersion">The last row version number we processed.</param>
    Task<List<Hotel>> GetChangedRecordsInAscendingOrderAsync(byte[] highWaterMarkRowVersion);
}
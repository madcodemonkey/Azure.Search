using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Repositories;

public interface IHotelRepository : IRepositoryPrimaryKeyBase<Hotel, int>
{
    /// <summary>
    /// Gets the latest changes based on the row version number in ASCENDING order.
    /// </summary>
    /// <param name="highWaterMarkRowVersion">The last row version number we processed.</param>
    /// <param name="retrievalLimit">The maximum number of records to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<List<Hotel>> GetChangedRecordsInAscendingOrderAsync(byte[] highWaterMarkRowVersion,
        int retrievalLimit = Int32.MaxValue, CancellationToken cancellationToken = default);

}
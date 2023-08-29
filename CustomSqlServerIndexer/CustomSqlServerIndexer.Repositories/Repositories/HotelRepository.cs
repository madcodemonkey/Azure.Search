using CustomSqlServerIndexer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomSqlServerIndexer.Repositories;

public class HotelRepository : RepositoryPrimaryKeyBase<Hotel, CustomSqlServerContext, int>, IHotelRepository
{
    /// <summary>Constructor</summary>
    public HotelRepository(CustomSqlServerContext context) : base(context)
    {

    }

    /// <summary>
    /// Gets the latest changes based on the row version number in ASCENDING order.
    /// </summary>
    /// <param name="highWaterMarkRowVersion">The last row version number we processed.</param>
    /// <param name="retrievalLimit">The maximum number of records to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<List<Hotel>> GetChangedRecordsInAscendingOrderAsync(byte[] highWaterMarkRowVersion,
        int retrievalLimit = Int32.MaxValue, CancellationToken cancellationToken = default)
    {
        List<Hotel> result;

        string limitString = retrievalLimit == Int32.MaxValue ? string.Empty : $"Top({retrievalLimit})";

        // Note: Do not check for Array.Empty<byte>, it will NOT catch an array that was initialized with zero length (e.g., var myArray = new byte[0])
        if (highWaterMarkRowVersion.Length == 0)
        {
            result = await Context.Hotels
                .FromSqlRaw($"SELECT {limitString} * FROM {nameof(Hotel)} ORDER BY {nameof(Hotel.RowVersion)} ASC")
                .ToListAsync(cancellationToken);
        }
        else
        {
            // https://stackoverflow.com/a/20816340/97803
            // SQL Raw: https://learn.microsoft.com/en-us/ef/core/querying/sql-queries
            long rowVersionNumber = BitConverter.ToInt64(highWaterMarkRowVersion.Reverse().ToArray(), 0); //  0x00000000000007E4;

            result = await Context.Hotels
                .FromSqlRaw($"SELECT {limitString} * FROM {nameof(Hotel)} WHERE {nameof(Hotel.RowVersion)} > {rowVersionNumber} ORDER BY {nameof(Hotel.RowVersion)} ASC")
                .ToListAsync(cancellationToken);
        }
        
        return result;
    }
     
}
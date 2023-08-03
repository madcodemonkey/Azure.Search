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
    public async Task<List<Hotel>> GetChangedRecordsInAscendingOrderAsync(byte[] highWaterMarkRowVersion)
    {
        List<Hotel> result;

        if (highWaterMarkRowVersion == Array.Empty<byte>())
        {
            result = await Context.Hotels
                .FromSqlRaw($"SELECT * FROM {nameof(Hotel)} ORDER BY {nameof(Hotel.RowVersion)} ASC")
                .ToListAsync();
        }
        else
        {
            long rowVersionNumber = BitConverter.ToInt64(highWaterMarkRowVersion.Reverse().ToArray(), 0); //  0x00000000000007E4;

            result = await Context.Hotels
                .FromSqlRaw($"SELECT * FROM {nameof(Hotel)} WHERE {nameof(Hotel.RowVersion)} > {rowVersionNumber} ORDER BY {nameof(Hotel.RowVersion)} ASC")
                .ToListAsync();
        }
        
        return result;
    }
     
}
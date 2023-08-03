using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;
using Microsoft.Extensions.Logging;

namespace CustomSqlServerIndexer.Services;

public class CustomSqlServerIndexerService : ICustomSqlServerIndexerService
{
    private readonly ILogger<CustomSqlServerIndexerService> _logger;
    private readonly IHotelRepository _hotelRepository;
    private readonly IHighWaterMarkStorageService _highWaterMarkStorage;

    public CustomSqlServerIndexerService(ILogger<CustomSqlServerIndexerService> logger, 
        IHotelRepository hotelRepository, IHighWaterMarkStorageService highWaterMarkStorage)
    {
        _logger = logger;
        _hotelRepository = hotelRepository;
        _highWaterMarkStorage = highWaterMarkStorage;
    }

    public async Task DoWorkAsync()
    {
        byte[] highWaterMarkRowVersion = await _highWaterMarkStorage.GetHighWaterMarkRowVersionAsync();

        List<Hotel> hotels = await _hotelRepository.GetChangedRecordsInAscendingOrderAsync(highWaterMarkRowVersion);

        _logger.LogInformation($"The SQL Indexer found {hotels.Count} changes.");

        if (hotels.Count == 0)
        {
            return;
        }

        // The last item in the list is the newest so use it to set the high water mark.
        await _highWaterMarkStorage.SetHighWaterMarkRowVersionAsync(hotels[^1].RowVersion);
        
        highWaterMarkRowVersion = await _highWaterMarkStorage.GetHighWaterMarkRowVersionAsync();

        hotels = await _hotelRepository.GetChangedRecordsInAscendingOrderAsync(highWaterMarkRowVersion);

        if (hotels.Count == 0)
        {
            _logger.LogInformation("Yes, it's empty like it supposed to be.");
        }
        else
        {
            _logger.LogInformation("No, it is NOT empty!");
        }
    }
}
 
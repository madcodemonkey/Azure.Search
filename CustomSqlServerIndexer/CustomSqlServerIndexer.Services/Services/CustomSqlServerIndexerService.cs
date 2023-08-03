using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;
using Microsoft.Extensions.Logging;

namespace CustomSqlServerIndexer.Services;

public class CustomSqlServerIndexerService : ICustomSqlServerIndexerService
{
    private readonly ILogger<CustomSqlServerIndexerService> _logger;
    private readonly ICustomSearchIndexService _cognitiveIndexService;
    private readonly IHotelRepository _hotelRepository;
    private readonly IHighWaterMarkStorageService _highWaterMarkStorage;
    private readonly Random _random = new Random(DateTime.Now.Millisecond);

    public CustomSqlServerIndexerService(ILogger<CustomSqlServerIndexerService> logger,
        ICustomSearchIndexService cognitiveIndexService,
        IHotelRepository hotelRepository, IHighWaterMarkStorageService highWaterMarkStorage)
    {
        _logger = logger;
        _cognitiveIndexService = cognitiveIndexService;
        _hotelRepository = hotelRepository;
        _highWaterMarkStorage = highWaterMarkStorage;
    }

    /// <summary>
    /// Does the work necessary when changes are found in the database.
    /// </summary>
    /// <returns>The number of changes made to the index.</returns>
    public async Task<int> DoWorkAsync(CancellationToken cancellationToken)
    {
        byte[] highWaterMarkRowVersion = await _highWaterMarkStorage.GetHighWaterMarkRowVersionAsync();

        List<Hotel> hotels = await _hotelRepository.GetChangedRecordsInAscendingOrderAsync(highWaterMarkRowVersion);

        _logger.LogInformation($"The SQL Indexer found {hotels.Count} changes.");

        if (hotels.Count == 0)
        {
            return 0;
        }

        int successfulChanges = 0;
        foreach (var hotel in hotels)
        {
            try
            {
                if (hotel.IsDeleted)
                {
                    await ProcessDeletionAsync(hotel);
                }
                else
                {
                    await ProcessChangeAsync(hotel);
                }

                // Since the hotels are sorted in the oldest to newest change order, we can update
                // this high watermark as we process each file.  
                await _highWaterMarkStorage.SetHighWaterMarkRowVersionAsync(hotels[^1].RowVersion);

                successfulChanges++;
            }
            catch (Exception ex)
            {
                // TODO: Change this logic to so that we queue the change error so that we can continue to the next item.
                // If we JUST update the high watermark and go to the next record, the change will be lost and never processed again till the record changes.
                _logger.LogError(ex, $"Failed to process the hotel record with id {hotel.Id}!  We will try again when the timer next fires!");
                break; 
            }
        }

        return successfulChanges;
    }

    /// <summary>
    /// Deletes a record from the Azure Cognitive Search index.
    /// </summary>
    private async Task ProcessDeletionAsync(Hotel hotel)
    {
        await _cognitiveIndexService.DeleteDocumentsAsync(nameof(SearchIndexDocument.HotelId),
            new List<string>() { hotel.Id.ToString() });
    }

    /// <summary>
    /// Creates or Updates records in the Azure Cognitive Search index.
    /// </summary>
    private async Task ProcessChangeAsync(Hotel hotel)
    {
        var indexDocument = new SearchIndexDocument
        {
            HotelId = hotel.Id.ToString(),
            BaseRate = hotel.BaseRate,
            Category = hotel.Category ?? "None",
            Description = hotel.Description ?? "None",
            DescriptionFr = hotel.DescriptionFr ?? "rien",
            HotelName = hotel.HotelName,
            IsDeleted = hotel.IsDeleted,
            LastRenovationDate = hotel.LastRenovationDate,
            ParkingIncluded = hotel.ParkingIncluded,
            Rating = hotel.Rating,
            Roles = CreateRandomRoles(),
            SmokingAllowed = hotel.SmokingAllowed,
            Tags = hotel.Amenities?.Split(',') ?? Array.Empty<string>(),
        };


        await _cognitiveIndexService.UploadDocumentsAsync(indexDocument);
    }

    private string[] CreateRandomRoles()
    {
        var result = new List<string> { "Admin" };

        if (_random.Next(1, 100) > 50)
        {
            result.Add("Guest");
        }

        if (_random.Next(1, 100) > 50)
        {
            result.Add("Member");
        }

        return result.ToArray();
    }
}
 
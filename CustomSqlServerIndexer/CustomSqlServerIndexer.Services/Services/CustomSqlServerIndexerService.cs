using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CustomSqlServerIndexer.Services;

public class CustomSqlServerIndexerService : ICustomSqlServerIndexerService
{
    private readonly ILogger<CustomSqlServerIndexerService> _logger;
    private readonly CustomIndexerSettings _settings;
    private readonly ICustomSearchIndexService _cognitiveIndexService;
    private readonly IHotelRepository _hotelRepository;
    private readonly IHighWaterMarkStorageService _highWaterMarkStorage;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomSqlServerIndexerService(ILogger<CustomSqlServerIndexerService> logger,
        IOptions<CustomIndexerSettings> serviceSettings,
        ICustomSearchIndexService cognitiveIndexService,
        IHotelRepository hotelRepository, IHighWaterMarkStorageService highWaterMarkStorage)
    {
        _logger = logger;
        _settings = serviceSettings.Value;
        _cognitiveIndexService = cognitiveIndexService;
        _hotelRepository = hotelRepository;
        _highWaterMarkStorage = highWaterMarkStorage;
    }

    /// <summary>
    /// Does the work necessary when changes are found in the database.
    /// </summary>
    /// <param name="retrievalLimit">The maximum number of records to retrieve</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of changes made to the index.</returns>
    public async Task<int> DoWorkAsync(int retrievalLimit = Int32.MaxValue, CancellationToken cancellationToken = default)
    {
        byte[] highWaterMarkRowVersion = await _highWaterMarkStorage.GetHighWaterMarkRowVersionAsync();

        List<Hotel> hotels = await _hotelRepository.GetChangedRecordsInAscendingOrderAsync(
            highWaterMarkRowVersion, retrievalLimit, cancellationToken);

        _logger.LogInformation($"The SQL Indexer found {hotels.Count} changes.");

        if (hotels.Count == 0)
        {
            return 0;
        }

        int numberOfItemsChanged = 0;
        List<string> listOfKeysToDelete = new List<string>();
        List<SearchIndexDocument> listOfChangedItems = new List<SearchIndexDocument>();
        
        foreach (var hotel in hotels)
        {
            try
            {
                if (hotel.IsDeleted)
                {
                    listOfKeysToDelete.Add(hotel.Id.ToString());
                }
                else
                {
                    listOfChangedItems.Add(MapDocument(hotel));
                }

                if ((listOfKeysToDelete.Count + listOfChangedItems.Count) >= _settings.CognitiveSearchMaxUpsertBatchSize)
                {
                    numberOfItemsChanged += await SaveBatchAsync(listOfKeysToDelete, listOfChangedItems, cancellationToken);

                    // Since the hotels are sorted in the oldest to newest change order, we can update
                    // this high watermark as we process files.  
                    await _highWaterMarkStorage.SetHighWaterMarkRowVersionAsync(hotel.RowVersion);
                }

            }
            catch (Exception ex)
            {
                // TODO: Change this logic to so that we queue the change error so that we can continue to the next item.
                // If we JUST update the high watermark and go to the next record, the change will be lost and never processed again till the record changes.
                _logger.LogError(ex, $"Failed to process the hotel record with id {hotel.Id}!  We will try again when the timer next fires!");
                break; 
            }
        }

        numberOfItemsChanged += await SaveBatchAsync(listOfKeysToDelete, listOfChangedItems, cancellationToken);

        // Since the hotels are sorted in the oldest to newest change order, we can update
        // this high watermark as we process each file.  
        await _highWaterMarkStorage.SetHighWaterMarkRowVersionAsync(hotels[^1].RowVersion);

        return numberOfItemsChanged;
    }

    private async Task<int> SaveBatchAsync(List<string> listOfKeysToDelete,
        List<SearchIndexDocument> listOfChangedItems, CancellationToken cancellationToken)
    {
        if (listOfKeysToDelete.Any())
        {
            await _cognitiveIndexService.DeleteDocumentsAsync(nameof(SearchIndexDocument.HotelId),
                listOfKeysToDelete, cancellationToken);
        }

        if (listOfChangedItems.Any())
        {
            await _cognitiveIndexService.UploadDocumentsAsync(listOfChangedItems, cancellationToken);
        }

        int changedItems = listOfKeysToDelete.Count + listOfChangedItems.Count;

        listOfKeysToDelete.Clear();
        listOfChangedItems.Clear();

        return changedItems;
    }

    /// <summary>
    /// Creates or Updates records in the Azure Cognitive Search index.
    /// </summary>
    private SearchIndexDocument MapDocument(Hotel hotel)
    { 
        return new SearchIndexDocument
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
            Roles = hotel.Roles.Split(',').ToArray(),
            SmokingAllowed = hotel.SmokingAllowed,
            Tags = hotel.Amenities?.Split(',') ?? Array.Empty<string>(),
        };
    }
}
 
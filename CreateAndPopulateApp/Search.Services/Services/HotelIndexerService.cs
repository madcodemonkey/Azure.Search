using Azure.Search.Documents.Indexes.Models;

namespace Search.Services;

public class HotelIndexerService : IHotelIndexerService
{
    private readonly ISearchIndexerService _indexerService;

    /// <summary>Constructor</summary>
    public HotelIndexerService(ISearchIndexerService indexerService)
    {
        _indexerService = indexerService;
    }

    public async Task CreateIndexerAsync(string indexerName, string dataSourceName, string targetIndexName)
    {
        
        var schedule = new IndexingSchedule(TimeSpan.FromDays(1))
        {
            StartTime = DateTimeOffset.Now
        };

        var parameters = new IndexingParameters()
        {
            BatchSize = 100,
            MaxFailedItems = 0,
            MaxFailedItemsPerBatch = 0
        };

        // Indexer declarations require a data source and search index.
        // Common optional properties include a schedule, parameters, and field mappings
        // The field mappings below are redundant due to how the Hotel class is defined, but 
        // we included them anyway to show the syntax 
        var indexer = new SearchIndexer(indexerName, dataSourceName, targetIndexName)
        {
            Description = "Data indexer",
            Schedule = schedule,
            Parameters = parameters,
            FieldMappings =
            {
                new FieldMapping("Id") {TargetFieldName = "HotelId"},
                new FieldMapping("Amenities") {TargetFieldName = "Tags"}
            }
        };

        await _indexerService.ClientIndexer.CreateOrUpdateIndexerAsync(indexer);
    }

}
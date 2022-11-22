using Azure.Search.Documents.Indexes.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelIndexerService : AcmeSearchIndexerService, IHotelIndexerService
{
    private readonly SearchServiceSettings _searchSettings;

    /// <summary>Constructor</summary>
    public HotelIndexerService(SearchServiceSettings settings) : base(settings)
    {
        _searchSettings = settings;
    }
   
    /// <summary>Creates the Hotel indexer</summary>
    public async Task<bool> CreateAsync()
    {
        // How often should the indexer run?
        var schedule = new IndexingSchedule(TimeSpan.FromDays(1))
        {
            StartTime = DateTimeOffset.Now
        };

        var parameters = new IndexingParameters
        {
            BatchSize = 100,
            MaxFailedItems = 0,
            MaxFailedItemsPerBatch = 0
        };

        // Indexer declarations require a data source and search index.
        // Common optional properties include a schedule, parameters, and field mappings
        // The field mappings below are redundant due to how the HotelDocument class is defined, but 
        // we included them anyway to show the syntax 
        var indexer = new SearchIndexer(_searchSettings.Hotel.IndexerName, _searchSettings.Hotel.DataSourceName, _searchSettings.Hotel.IndexName)
        {
            Description = "Hotel data indexer",
            Schedule = schedule,
            Parameters = parameters,
            FieldMappings =
            {
                new FieldMapping("Id") {TargetFieldName = nameof(HotelDocument.HotelId)},
                new FieldMapping("Amenities") {TargetFieldName = nameof(HotelDocument.Tags) }
            }
        };


        var data = await ClientIndexer.CreateOrUpdateIndexerAsync(indexer);

        return data != null;  // TODO: Is this a good check?
    }

    /// <summary>Deletes the hotel indexer</summary>
    public async Task<bool> DeleteAsync()
    {
        return await DeleteAsync(_searchSettings.Hotel.IndexerName);
    }

    /// <summary>Runs the hotel indexer now.</summary>
    public async Task RunAsync()
    {
        await ClientIndexer.RunIndexerAsync(_searchSettings.Hotel.IndexerName);
    }

}
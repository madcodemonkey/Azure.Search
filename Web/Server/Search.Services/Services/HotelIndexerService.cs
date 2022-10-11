using Azure.Search.Documents.Indexes.Models;
using Search.Model;

namespace Search.Services;

public class HotelIndexerService : SearchIndexerService, IHotelIndexerService
{
    /// <summary>Constructor</summary>
    public HotelIndexerService(SearchServiceSettings settings) : base(settings)
    {
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
        // The field mappings below are redundant due to how the SearchHotel class is defined, but 
        // we included them anyway to show the syntax 
        var indexer = new SearchIndexer(Settings.HotelIndexerName, Settings.HotelDataSourceName, Settings.HotelIndexName)
        {
            Description = "Hotel data indexer",
            Schedule = schedule,
            Parameters = parameters,
            FieldMappings =
            {
                new FieldMapping("Id") {TargetFieldName = nameof(SearchHotel.HotelId)},
                new FieldMapping("Amenities") {TargetFieldName = nameof(SearchHotel.Tags) }
            }
        };


        var data = await ClientIndexer.CreateOrUpdateIndexerAsync(indexer);

        return data != null;  // TODO: Is this a good check?
    }

    /// <summary>Deletes the hotel indexer</summary>
    public async Task<bool> DeleteAsync()
    {
        return await DeleteAsync(Settings.HotelIndexerName);
    }

    /// <summary>Runs the hotel indexer now.</summary>
    public async Task RunAsync()
    {
        await ClientIndexer.RunIndexerAsync(Settings.HotelIndexerName);
    }

}
using Azure.Search.Documents.Indexes.Models;
using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;

namespace CustomSqlServerIndexer.Services;

public class GremlinCogSearchService  : IGremlinCogSearchService
{
    private readonly ServiceSettings _serviceSettings;
    private readonly RepositorySettings _repositorySettings;
    private readonly ICogClientWrapperService _clientService;
    private readonly ICustomSearchIndexService _searchIndexService;
    private readonly ICogSearchDataSourceService _dataSourceService;

    /// <summary>
    /// Constructor
    /// </summary>
    public GremlinCogSearchService(ServiceSettings serviceSettings, RepositorySettings repositorySettings,
        ICogClientWrapperService clientService,
        ICogSearchDataSourceService dataSourceService,
        ICustomSearchIndexService searchIndexService)
    {
        _serviceSettings = serviceSettings;
        _repositorySettings = repositorySettings;
        _clientService = clientService;
        _dataSourceService = dataSourceService;
        _searchIndexService = searchIndexService;
    }

    /// <summary>
    /// Create the out-of-the-box Apache Gremlin indexER.
    /// </summary>
    public async Task CreateAsync()
    {
        _searchIndexService.CreateOrUpdateIndex();
        await CreateDataSource();
        await CreateIndexerAsync();
    }

    private async Task CreateDataSource()
    {
     //   if (await _dataSourceService.ExistsAsync(_serviceSettings.CognitiveSearchDataSourceName) == false)
        
            await _dataSourceService.CreateForGremlinAsync(_serviceSettings.CognitiveSearchDataSourceName,
                _repositorySettings.GremlinContainerName,
                _repositorySettings.GremlinDatabaseConnectionString,
                GremlinQueryTypes.Vertices,
                null,
                null);
        
    }


    /// <summary>Creates the Hotel indexer</summary>
    private async Task<bool> CreateIndexerAsync()
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
        var indexer = new SearchIndexer(_serviceSettings.CognitiveSearchIndexerName, _serviceSettings.CognitiveSearchDataSourceName, _serviceSettings.CognitiveSearchIndexName)
        {
            Description = "Gremlin data indexer",
            Schedule = schedule,
            Parameters = parameters,
            //FieldMappings =
            //{
            //    new FieldMapping("id") {TargetFieldName = nameof(SearchIndexDocument.Id)},
            //    new FieldMapping("label") {TargetFieldName = nameof(SearchIndexDocument.Label)},
            //},
            OutputFieldMappings =
            {
                new FieldMapping("/document/firstName") {TargetFieldName = nameof(SearchIndexDocument.FirstName)},
                new FieldMapping("/document/age") {TargetFieldName = nameof(SearchIndexDocument.Age) }
            }
        };

        // Field mappings:
        // - Docs: https://learn.microsoft.com/en-us/azure/search/search-indexer-field-mappings?tabs=rest
        // - If I needed to rename something that was already in the source doc, I could do that here.
        // - Top-level search fields only, where the "targetFieldName" is either a simple field or a collection. A target field can't be a complex type.
        //   In the case of firstName and age above, they are complex types so I use output field mappings!
        // - Things like id, label which only differ by case, don't need to be mapped since the IndexER will figure it out on its own.

        // Output field mappings
        // - Docs: https://learn.microsoft.com/en-us/azure/search/cognitive-search-output-field-mapping?tabs=rest

        var clientIndexer = _clientService.GetIndexerClient();
        var data = await clientIndexer.CreateOrUpdateIndexerAsync(indexer);

        return data != null;  // TODO: Is this a good check?
    }
}
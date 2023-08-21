using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using CogSimple.Services;
using CustomSqlServerIndexer.Models;
using CustomSqlServerIndexer.Repositories;
using CustomSqlServerIndexer.Services;


namespace OutOfTheBoxGremlinIndexer.Services;

public class OutOfBoxService : IOutOfBoxService
{
    private readonly ICogClientWrapperService _clientService;
    private readonly ICogSearchIndexService _indexService;
    private readonly ICogSearchIndexerService _indexerService;
    private readonly ICogSearchDataSourceService _dataSourceService;
    private readonly ServiceSettings _serviceSettings;
    private readonly RepositorySettings _repositorySettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public OutOfBoxService(ServiceSettings serviceSettings, RepositorySettings repositorySettings,
        ICogClientWrapperService clientService,
        ICogSearchIndexService indexService,
        ICogSearchIndexerService indexerService,
        ICogSearchDataSourceService dataSourceService)
    {
        _serviceSettings = serviceSettings;
        _repositorySettings = repositorySettings;
        _clientService = clientService;
        _indexService = indexService;
        _indexerService = indexerService;
        _dataSourceService = dataSourceService;
    }

    /// <summary>
    /// Create the index, data source and indexer for an out-of-the-box blob solution.
    /// </summary>
    public async Task CreateAsync(CancellationToken cancellationToken = default)
    {
        await CreateIndexAsync(cancellationToken);
        await CreateDataSourceAsync(cancellationToken);
        await CreateIndexerAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the index, data source and indexer for the out-of-the-box blob solution.
    /// </summary>
    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await _indexService.DeleteIndexAsync(_serviceSettings.CognitiveSearchIndexName, checkIfExistsFirst: true, cancellationToken: cancellationToken);
        await _indexerService.DeleteIndexerAsync(_serviceSettings.CognitiveSearchIndexerName, checkIfExistsFirst: true, cancellationToken: cancellationToken);
        await _dataSourceService.DeleteAsync(_serviceSettings.CognitiveSearchDataSourceName, checkIfExistsFirst:true, cancellationToken);
    }

    /// <summary>
    /// Runs the indexer
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task RunIndexerAsync(CancellationToken cancellationToken = default)
    {
        var clientIndexer = _clientService.GetIndexerClient();
        await clientIndexer.RunIndexerAsync(_serviceSettings.CognitiveSearchIndexerName, cancellationToken);
    }
    
    /// <summary>Creates a Cosmos DB Apache Gremlin data source that will be used by an indexer.</summary>
    /// <remarks>
    /// See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#define-the-data-source
    /// </remarks>
    private async Task<bool> CreateDataSourceAsync(CancellationToken cancellationToken = default)
    {
        //   if (await _dataSourceService.ExistsAsync(_serviceSettings.CognitiveSearchDataSourceName) == false)

        /*
         *
        {
  "@odata.context": "https://yatescloudsearch.search.windows.net/$metadata#datasources/$entity",
  "@odata.etag": "\"0x8DB95359F5784FA\"",
  "name": "my-gremlin-ds",
  "description": null,
  "type": "cosmosdb",
  "subtype": "Gremlin",   // <----------------------------  Can't find the sub-type.
  "credentials": {
    "connectionString": "AccountEndpoint=https://yatesgremlin.documents.azure.com;AccountKey=...;Database=sample-database;ApiKind=Gremlin;"
  },
  
  "dataDeletionDetectionPolicy": null,
  "encryptionKey": null,
  "identity": null
}
         */


         
        // Vertices = 1, Edges
        var searchIndexerDataContainer = new SearchIndexerDataContainer(_repositorySettings.GremlinContainerName)
        {
            Query = _repositorySettings.GremlinQueryType.ToLower() == "vertices" ? "g.V()": "g.E()"
        };
        
        
        var dataSource = new SearchIndexerDataSourceConnection(_serviceSettings.CognitiveSearchDataSourceName,
            SearchIndexerDataSourceType.CosmosDb,
            _repositorySettings.GremlinDatabaseConnectionString,
            searchIndexerDataContainer)
        {
            DataChangeDetectionPolicy = new HighWaterMarkChangeDetectionPolicy("_ts"),
        };

        

        if (!string.IsNullOrWhiteSpace(_repositorySettings.GremlinSoftDeleteColumnName) && !string.IsNullOrWhiteSpace(_repositorySettings.GremlinSoftDeleteColumnValue))
        {
            dataSource.DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
            {
                SoftDeleteColumnName = _repositorySettings.GremlinSoftDeleteColumnName,
                SoftDeleteMarkerValue = _repositorySettings.GremlinSoftDeleteColumnValue
            };
        }


        // The data source does not need to be deleted if it was already created,
        // but the connection string may need to be updated if it was changed
        var indexerClient = _clientService.GetIndexerClient();
        var results = await indexerClient.CreateOrUpdateDataSourceConnectionAsync(dataSource, cancellationToken: cancellationToken);

        return results != null;  // Is this a good check?
    }



    /// <summary>
    /// Create index or update the index.
    /// </summary>
    private async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {

        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_serviceSettings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester(_serviceSettings.CognitiveSearchSuggestorName,
            new[] { nameof(SearchIndexDocument.FirstName), nameof(SearchIndexDocument.Label) });
        definition.Suggesters.Add(suggester);


        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.FirstName)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.FirstName) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Label) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_serviceSettings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = _clientService.GetIndexClient();
        await indexClient.CreateOrUpdateIndexAsync(definition, cancellationToken: cancellationToken);
    }

    /// <summary>Creates the blob indexer</summary>
    private async Task<bool> CreateIndexerAsync(CancellationToken cancellationToken = default)
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
        var data = await clientIndexer.CreateOrUpdateIndexerAsync(indexer, cancellationToken: cancellationToken);

        return data != null;  // TODO: Is this a good check?
    }
}
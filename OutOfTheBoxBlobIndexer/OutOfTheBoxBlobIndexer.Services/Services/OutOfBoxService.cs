using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using OutOfTheBoxBlobIndexer.Models;


namespace OutOfTheBoxBlobIndexer.Services.Services;

public class OutOfBoxService : IOutOfBoxService
{
    private readonly ICogClientWrapperService _clientService;
    private readonly ICogSearchIndexService _indexService;
    private readonly ICogSearchDataSourceService _dataSourceService;
    private readonly ServiceSettings _serviceSettings;
    /// <summary>
    /// Constructor
    /// </summary>
    public OutOfBoxService(ServiceSettings serviceSettings,
        ICogClientWrapperService clientService,
        ICogSearchIndexService indexService,
        ICogSearchDataSourceService dataSourceService)
    {
        _serviceSettings = serviceSettings;
        _clientService = clientService;
        _indexService = indexService;
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
        await _indexService.DeleteIndexAsync(_serviceSettings.CognitiveSearchIndexName);

        var indexerClient = _clientService.GetIndexerClient();
        await indexerClient.DeleteIndexerAsync(_serviceSettings.CognitiveSearchIndexerName, cancellationToken);

        await _dataSourceService.DeleteAsync(_serviceSettings.CognitiveSearchDataSourceName);
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

    private async Task CreateDataSourceAsync(CancellationToken cancellationToken = default)
    {
        //   if (await _dataSourceService.ExistsAsync(_serviceSettings.CognitiveSearchDataSourceName) == false)

        await _dataSourceService.CreateForBlobAsync(_serviceSettings.CognitiveSearchDataSourceName,
            _serviceSettings.StorageContainerName,
            _serviceSettings.StorageConnectionString,
            _serviceSettings.CognitiveSearchSoftDeleteFieldName,
            _serviceSettings.CognitiveSearchSoftDeleteFieldValue,
            cancellationToken);
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
            new[]
        {
            nameof(SearchIndexDocument.FileName),
            nameof(SearchIndexDocument.ContentType)
        });
        definition.Suggesters.Add(suggester);


        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.FileName)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Content) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.FileName) });

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
            Description = "Blob data indexer",
            Schedule = schedule,
            Parameters = parameters,
            FieldMappings =
            {
                new FieldMapping("metadata_storage_path") {TargetFieldName = nameof(SearchIndexDocument.Id), MappingFunction = new FieldMappingFunction("base64Encode")},
                new FieldMapping("metadata_content_type") {TargetFieldName = nameof(SearchIndexDocument.ContentType)},
                new FieldMapping("metadata_storage_file_extension") {TargetFieldName = nameof(SearchIndexDocument.FileExtension)},
                new FieldMapping("metadata_storage_name") {TargetFieldName = nameof(SearchIndexDocument.FileName)},
            },
            //OutputFieldMappings =
            //{
            //    new FieldMapping("/document/firstName") {TargetFieldName = nameof(SearchIndexDocument.FirstName)},
            //    new FieldMapping("/document/age") {TargetFieldName = nameof(SearchIndexDocument.Age) }
            //}
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
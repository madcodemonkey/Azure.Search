using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using CogSimple.Services;
using Microsoft.Extensions.Options;
using OutOfTheBoxBlobIndexer.Models;

namespace OutOfTheBoxBlobIndexer.Services;

public class OutOfBoxService : IOutOfBoxService
{
    private readonly CognitiveSettings _cognitiveSettings;
    private readonly ICogClientWrapperService _clientService;
    private readonly ICogSearchIndexService _indexService;
    private readonly ICogSearchIndexerService _indexerService;
    private readonly ICogSearchDataSourceService _dataSourceService;
    private readonly ICogSearchSkillSetService _skillSetService;
    private readonly OutOfTheBoxSettings _outOfTheBoxSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public OutOfBoxService(IOptions<OutOfTheBoxSettings> serviceSettings,
        IOptions<CognitiveSettings> cognitiveSettings,
        ICogClientWrapperService clientService,
        ICogSearchIndexService indexService,
        ICogSearchIndexerService indexerService,
        ICogSearchDataSourceService dataSourceService,
        ICogSearchSkillSetService skillSetService)
    {
        _outOfTheBoxSettings = serviceSettings.Value;
        _cognitiveSettings = cognitiveSettings.Value;
        _clientService = clientService;
        _indexService = indexService;
        _indexerService = indexerService;
        _dataSourceService = dataSourceService;
        _skillSetService = skillSetService;
    }

    /// <summary>
    /// Create the index, data source and indexer for an out-of-the-box blob solution.
    /// </summary>
    public async Task CreateAsync(CancellationToken cancellationToken = default)
    {
        await CreateIndexAsync(cancellationToken);
        await CreateDataSourceAsync(cancellationToken);
        await CreateSkillSetAsync(cancellationToken);
        await CreateIndexerAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the index, data source and indexer for the out-of-the-box blob solution.
    /// </summary>
    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await _indexService.DeleteIndexAsync(_outOfTheBoxSettings.CognitiveSearchIndexName, checkIfExistsFirst: true, cancellationToken: cancellationToken);
        await _indexerService.DeleteIndexerAsync(_outOfTheBoxSettings.CognitiveSearchIndexerName, checkIfExistsFirst: true, cancellationToken: cancellationToken);
        await _dataSourceService.DeleteAsync(_outOfTheBoxSettings.CognitiveSearchDataSourceName, checkIfExistsFirst: true, cancellationToken);
        await _skillSetService.DeleteAsync(_outOfTheBoxSettings.CognitiveSearchSkillSetName, checkIfExistsFirst: true, cancellationToken);
    }

    /// <summary>
    /// Runs the indexer
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task RunIndexerAsync(CancellationToken cancellationToken = default)
    {
        var clientIndexer = _clientService.GetIndexerClient();
        await clientIndexer.RunIndexerAsync(_outOfTheBoxSettings.CognitiveSearchIndexerName, cancellationToken);
    }

    private async Task CreateDataSourceAsync(CancellationToken cancellationToken = default)
    {
        //   if (await _dataSourceService.ExistsAsync(_outOfTheBoxSettings.CognitiveSearchDataSourceName) == false)

        await _dataSourceService.CreateForBlobAsync(_outOfTheBoxSettings.CognitiveSearchDataSourceName,
            _outOfTheBoxSettings.StorageContainerName,
            _outOfTheBoxSettings.StorageConnectionString,
            _outOfTheBoxSettings.CognitiveSearchSoftDeleteFieldName,
            _outOfTheBoxSettings.CognitiveSearchSoftDeleteFieldValue,
            cancellationToken);
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    private async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_outOfTheBoxSettings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester(_outOfTheBoxSettings.CognitiveSearchSuggestorName,
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

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_outOfTheBoxSettings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = _clientService.GetIndexClient();
        await indexClient.CreateOrUpdateIndexAsync(definition, cancellationToken: cancellationToken);
    }

    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Example: https://learn.microsoft.com/en-us/azure/search/cognitive-search-tutorial-blob-dotnet#language-detection-skill
    /// </remarks>
    private LanguageDetectionSkill CreateLanguageDetectionSkill()
    {
        var inputMappings = new List<InputFieldMappingEntry>
        {
            new InputFieldMappingEntry("text")
            {
                Source = "/document/content"
            }
        };

        var outputMappings = new List<OutputFieldMappingEntry>
        {
            new OutputFieldMappingEntry("languageCode")
            {
                TargetName = "languageCode"
            }
        };

        var languageDetectionSkill = new LanguageDetectionSkill(inputMappings, outputMappings)
        {
            Description = "Detect the language used in the document",
            Context = "/document"
        };

        return languageDetectionSkill;
    }

    /// <summary>
    /// Creates a skill set that will be used by the indexer
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <remarks>
    /// Example: https://learn.microsoft.com/en-us/azure/search/cognitive-search-tutorial-blob-dotnet#build-and-create-the-skillset
    /// </remarks>
    private async Task<SearchIndexerSkillset> CreateSkillSetAsync(CancellationToken cancellationToken = default)
    {
        var skills = new List<SearchIndexerSkill>
        {
            CreateLanguageDetectionSkill()
        };

        bool hasValidServiceKey = !string.IsNullOrWhiteSpace(_cognitiveSettings.ServicesKey) &&
                                  !_cognitiveSettings.ServicesKey.StartsWith("---");

        SearchIndexerSkillset skillset = new SearchIndexerSkillset(_outOfTheBoxSettings.CognitiveSearchSkillSetName, skills)
        {
            Description = "Out of the box blob skillset",
            CognitiveServicesAccount = hasValidServiceKey ? new CognitiveServicesAccountKey(_cognitiveSettings.ServicesKey) : null
        };

        var clientIndexer = _clientService.GetIndexerClient();

        // Create the skillset in your search service.
        // The skillset does not need to be deleted if it was already created
        // since we are using the CreateOrUpdate method
        await clientIndexer.CreateOrUpdateSkillsetAsync(skillset, cancellationToken: cancellationToken);

        return skillset;
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
        var indexer = new SearchIndexer(_outOfTheBoxSettings.CognitiveSearchIndexerName, _outOfTheBoxSettings.CognitiveSearchDataSourceName, _outOfTheBoxSettings.CognitiveSearchIndexName)
        {
            Description = "Blob data indexer",
            Schedule = schedule,
            SkillsetName = _outOfTheBoxSettings.CognitiveSearchSkillSetName,
            Parameters = parameters,
            FieldMappings =
            {
                new FieldMapping("metadata_storage_path") {TargetFieldName = nameof(SearchIndexDocument.Id), MappingFunction = new FieldMappingFunction("base64Encode")},
                new FieldMapping("metadata_content_type") {TargetFieldName = nameof(SearchIndexDocument.ContentType)},
                new FieldMapping("metadata_storage_file_extension") {TargetFieldName = nameof(SearchIndexDocument.FileExtension)},
                new FieldMapping("metadata_storage_name") {TargetFieldName = nameof(SearchIndexDocument.FileName)},
            },
            OutputFieldMappings =
            {
                // Note: the source is determined by the CreateLanguageDetectionSkill in the output mapping list.
                new FieldMapping("/document/languageCode") {TargetFieldName = nameof(SearchIndexDocument.Language)}
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
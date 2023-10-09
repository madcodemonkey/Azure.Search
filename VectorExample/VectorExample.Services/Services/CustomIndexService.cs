using Azure.Search.Documents.Indexes.Models;
using VectorExample.Models;
using Azure.Search.Documents.Indexes;
using CogSimple.Services;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

public class CustomIndexService : ICustomIndexService
{
    private readonly ICogClientWrapperService _clientService;
    private readonly ICogSearchIndexService _indexService;
    private readonly ApplicationSettings _appSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomIndexService(IOptions<ApplicationSettings> appSettings,
        ICogClientWrapperService clientService,
        ICogSearchIndexService indexService)
    {
        _appSettings = appSettings.Value;
        _clientService = clientService;
        _indexService = indexService;
    }

    /// <summary>
    /// Create the index, data source and indexer for an out-of-the-box blob solution.
    /// </summary>
    public async Task CreateAsync(CancellationToken cancellationToken = default)
    {
        await CreateIndexAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes the index, data source and indexer for the out-of-the-box blob solution.
    /// </summary>
    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        await _indexService.DeleteIndexAsync(_appSettings.CognitiveSearchIndexName, checkIfExistsFirst: true, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    /// Create index or update the index.
    /// </summary>
    private async Task CreateIndexAsync(CancellationToken cancellationToken = default)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_appSettings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester("sg", new[]
        {
            nameof(SearchIndexDocument.Title)
        });
        definition.Suggesters.Add(suggester);

        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.Title)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Content) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Category) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_appSettings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);
        
        // VECTOR fields
        definition.VectorSearch = new()
        {
            AlgorithmConfigurations =
            {
                new HnswVectorSearchAlgorithmConfiguration(_appSettings.VectorSearchConfigurationName)
            }
        };

        SetupVectorFieldProperties(definition, nameof(SearchIndexDocument.ContentVector));
        SetupVectorFieldProperties(definition, nameof(SearchIndexDocument.TitleVector));

        // Create it using the index client
        var indexClient = _clientService.GetIndexClient();
        await indexClient.CreateOrUpdateIndexAsync(definition, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Sets the vector field configuration and vector search dimensions for a field since it can't be done via the attribute notation.
    /// </summary>
    private void SetupVectorFieldProperties(SearchIndex definition, string fieldName)
    {
        SearchField? contentVectorField = definition.Fields.FirstOrDefault(w => w.Name == fieldName);
        if (contentVectorField != null)
        {
            contentVectorField.VectorSearchConfiguration = _appSettings.VectorSearchConfigurationName;
            contentVectorField.VectorSearchDimensions = 1536;
        }
    }
}
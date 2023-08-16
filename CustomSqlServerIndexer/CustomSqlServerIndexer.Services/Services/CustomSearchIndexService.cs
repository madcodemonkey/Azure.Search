using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using CogSimple.Services;
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public class CustomSearchIndexService : ICustomSearchIndexService
{
    private readonly ServiceSettings _settings;
    private readonly ICogClientWrapperService _clientWrapperService;
    private readonly ICogSearchIndexService _cogSearchIndexService;
   
    /// <summary>
    /// Constructor
    /// </summary>
    public CustomSearchIndexService(ServiceSettings settings,
        ICogClientWrapperService clientWrapperService,
        ICogSearchIndexService cogSearchIndexService)
    {
        _settings = settings;
        _clientWrapperService = clientWrapperService;
        _cogSearchIndexService = cogSearchIndexService;
    }


    /// <summary>
    /// Clears all documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteAllDocumentsAsync(string keyField, CancellationToken cancellationToken)
    {
        return await _cogSearchIndexService.DeleteAllDocumentsAsync(
            _settings.CognitiveSearchIndexName, keyField, cancellationToken);
    }

    /// <summary>
    /// Clears the specified documents from the index.
    /// </summary>
    /// <param name="keyField">The name of the key field that uniquely identifies documents in the index.</param>
    /// <param name="keys">The keys of the documents to delete.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of documents deleted</returns>
    public async Task<long> DeleteDocumentsAsync(string keyField, List<string> keys,
        CancellationToken cancellationToken)
    {
        return await _cogSearchIndexService.DeleteDocumentsAsync(_settings.CognitiveSearchIndexName,
            keyField, keys, cancellationToken);
    }

    /// <summary>
    /// Deletes the entire index and all it's documents!
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task DeleteIndexAsync(CancellationToken cancellationToken)
    {
         await _cogSearchIndexService.DeleteIndexAsync(_settings.CognitiveSearchIndexName, true, cancellationToken);
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task CreateOrUpdateIndexAsync(CancellationToken cancellationToken)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_settings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester("sg", new[]
        {
            nameof(SearchIndexDocument.HotelName), 
            nameof(SearchIndexDocument.Category)
        });
        definition.Suggesters.Add(suggester);


        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.HotelName)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.HotelName) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Description) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_settings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = _clientWrapperService.GetIndexClient(); 
        await indexClient.CreateOrUpdateIndexAsync(definition, cancellationToken: cancellationToken);
    }

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<SearchQueryResponse<T>> SearchAsync<T>(string searchText, SearchOptions options, CancellationToken cancellationToken) where T : class
    {
        return await _cogSearchIndexService.SearchAsync<T>(
            _settings.CognitiveSearchIndexName, searchText, options, cancellationToken);
    }

    /// <summary>
    /// Upload multiple documents in a single Upload request.
    /// </summary>
    /// <param name="docs">A list of docs to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task UploadDocumentsAsync(List<SearchIndexDocument> docs, CancellationToken cancellationToken)
    {
        await _cogSearchIndexService.UploadDocumentsAsync(_settings.CognitiveSearchIndexName,
            docs, cancellationToken);
    }
}
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public class CustomSearchIndexService : ICustomSearchIndexService
{
    private readonly ServiceSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomSearchIndexService(ServiceSettings settings)
    {
        _settings = settings;
    }


    /// <summary>
    /// Create index or update the index.
    /// </summary>
    public void CreateOrUpdateIndex()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));
        
        var definition = new SearchIndex(_settings.CognitiveSearchIndexName, searchFields);

        var suggester = new SearchSuggester("sg", new[] { "Title", "Id", "KeyPhrases" });
        definition.Suggesters.Add(suggester);

        var indexClient = GetIndexClient();
        indexClient.CreateOrUpdateIndex(definition);
    }

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    public void UploadDocuments(SearchIndexDocument doc)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));
        
        var searchClient = GetSearchClient();
        IndexDocumentsResult result = searchClient.IndexDocuments(batch);
    }

    private SearchIndexClient GetIndexClient()
    {
        if (_indexClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _indexClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _indexClient;
    }

    private SearchClient GetSearchClient()
    {
        if (_searchClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _searchClient = new SearchClient(serviceEndpoint, _settings.CognitiveSearchIndexName, credential);
        }

        return _searchClient;
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri($"https://{_settings.CognitiveSearchName}.search.windows.net/");
        return serviceEndpoint;
    }
}
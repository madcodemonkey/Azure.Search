using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

// TODO: Remove all Console.WriteLines 

public class CustomSearchIndexService : ICustomSearchIndexService
{
    private readonly ServiceSettings _settings;
    private SearchIndexClient? _adminClient;
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
        var searchFields = fieldBuilder.Build(typeof(Document));
        
        var definition = new SearchIndex(_settings.CognitiveSearchIndexName, searchFields);

        var suggester = new SearchSuggester("sg", new[] { "Title", "Id", "KeyPhrases" });
        definition.Suggesters.Add(suggester);

        var adminClient = GetAdminClient();
        adminClient.CreateOrUpdateIndex(definition);
    }

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    public void UploadDocuments(Document doc)
    {
        IndexDocumentsBatch<Document> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        try
        {
            var searchClient = GetSearchClient();
            IndexDocumentsResult result = searchClient.IndexDocuments(batch);
        }
        catch (Exception ex)
        {
            // If for some reason any documents are dropped during indexing, you can compensate by delaying and
            // retrying. This simple demo just logs the failed document keys and continues.
            Console.WriteLine($"Failed to index some of the documents: {ex.Message}");
        }
    }

    private SearchIndexClient GetAdminClient()
    {
        if (_adminClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _adminClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _adminClient;
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
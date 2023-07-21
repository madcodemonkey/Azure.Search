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
    /// Clears all documents from the index.
    /// </summary>
    public async Task<long> ClearAllDocumentsAsync()
    {
        try
        {
            var searchClient = GetSearchClient();

            long totalDeleted = 0;
            long totalCountOnLastTry = 0;
            long totalCountCurrent;

            do
            {
                var options = new SearchOptions
                {
                    Size = 50,
                    QueryType = SearchQueryType.Simple,
                    IncludeTotalCount = true
                };

                var azSearchResults = await this.SearchAsync<SearchDocument>("*", options);

                totalCountCurrent = azSearchResults.Value.TotalCount ?? 0;

                if (totalCountCurrent > 0)
                {
                    if (totalCountCurrent == totalCountOnLastTry)
                    {
                        // We are stuck and docs aren't be deleted!
                        break;
                    }

                    AsyncPageable<SearchResult<SearchDocument>> azOnePageOfSearchDocuments = azSearchResults.Value.GetResultsAsync();
                    var docsToDelete = new List<SearchDocument>();

                    await foreach (SearchResult<SearchDocument> item in azOnePageOfSearchDocuments)
                    {
                        docsToDelete.Add(item.Document);
                    }

                    await searchClient.DeleteDocumentsAsync(docsToDelete);

                    totalDeleted += docsToDelete.Count;
                }

                totalCountOnLastTry = totalCountCurrent;

            } while (totalCountCurrent > 0);

            return totalDeleted;
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
                return 0;
            throw;
        }
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

    /// <summary>Searches for documents</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<Response<SearchResults<T>>> SearchAsync<T>(string searchText, SearchOptions options)
    {
        var searchClient = GetSearchClient();
        var response = await searchClient.SearchAsync<T>(searchText, options);
        return response;
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
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace SearchServices.Services;

public class SearchClientService : ISearchClientService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public SearchClientService(SearchServiceSettings settings)
    {
        _settings = settings;
    }

    public SearchIndexClient GetIndexClient()
    {
        if (_indexClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _indexClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _indexClient;
    }

    public SearchClient GetSearchClient()
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
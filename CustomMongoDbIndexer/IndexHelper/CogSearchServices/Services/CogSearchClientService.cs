using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace CogSearchServices.Services;

public class CogSearchClientService : ICogSearchClientService
{
    private readonly CogSearchServiceSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public CogSearchClientService(CogSearchServiceSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Get the client that is used to manipulate the index.
    /// </summary>
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

    /// <summary>
    /// Get the client that is used to search the index.
    /// </summary>
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
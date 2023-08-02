using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace Search.CogServices;


 

/// <summary>
/// This is a wrapper around Microsoft client objects to help you create them.
/// </summary>
public class AcmeCogClientService : IAcmeCogClientService
{
    private readonly AcmeCogSettings _settings;
    private readonly SearchClientOptions _clientOptions;
    private SearchIndexClient? _indexClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public AcmeCogClientService(AcmeCogSettings settings, IAcmeCogOptionsService cogOptionsService)
    {
        _settings = settings;
        _clientOptions = cogOptionsService.CreateSearchClientOptions();
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
            _indexClient = new SearchIndexClient(serviceEndpoint, credential, _clientOptions);
        }
        
        return _indexClient;
    }

    /// <summary>
    /// Get the client that is used to search the index.
    /// </summary>
    public SearchClient GetSearchClient(string indexName)
    {
        if (_searchClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);
            _searchClient = new SearchClient(serviceEndpoint, indexName, credential);
        }

        return _searchClient;
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri(_settings.CognitiveSearchEndPoint);
        return serviceEndpoint;
    }
}
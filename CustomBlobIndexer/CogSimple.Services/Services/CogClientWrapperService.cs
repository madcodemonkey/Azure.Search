using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Options;

namespace CogSimple.Services;

public class CogClientWrapperService : ICogClientWrapperService
{
    private readonly CognitiveSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchIndexerClient? _indexerClient;
    private readonly Dictionary<string, SearchClient> _clients = new();

    /// <summary>
    /// Constructor
    /// </summary>
    public CogClientWrapperService(IOptions<CognitiveSettings> settings)
    {
        _settings = settings.Value;
    }


    public SearchIndexClient GetIndexClient()
    {
        if (_indexClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.SearchKey);
            _indexClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _indexClient;
    }

    public SearchIndexerClient GetIndexerClient()
    {
        if (_indexerClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.SearchKey);

            _indexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        return _indexerClient;
    }
    public SearchClient GetSearchClient(string indexName)
    {
        if (!_clients.ContainsKey(indexName))
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.SearchKey);
            var searchClient = new SearchClient(serviceEndpoint, indexName, credential);
            _clients.Add(indexName, searchClient);
        }

        return _clients[indexName];
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri(_settings.SearchEndpoint);
        return serviceEndpoint;
    }
}
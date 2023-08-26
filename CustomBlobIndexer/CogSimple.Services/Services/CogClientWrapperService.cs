using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Options;

namespace CogSimple.Services;

public class CogClientWrapperService : ICogClientWrapperService
{
    private readonly CogClientSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchIndexerClient? _indexerClient;
    private SearchClient? _searchClient;

    /// <summary>
    /// Constructor
    /// </summary>
    public CogClientWrapperService(IOptions<CogClientSettings> settings)
    {
        _settings = settings.Value;
    }


    public SearchIndexClient GetIndexClient()
    {
        if (_indexClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.Key);
            _indexClient = new SearchIndexClient(serviceEndpoint, credential);
        }

        return _indexClient;
    }

    public SearchIndexerClient GetIndexerClient()
    {
        if (_indexerClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.Key);

            _indexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        return _indexerClient;
    }
    public SearchClient GetSearchClient(string indexName)
    {
        if (_searchClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.Key);
            _searchClient = new SearchClient(serviceEndpoint, indexName, credential);
        }

        return _searchClient;
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri(_settings.Endpoint);
        return serviceEndpoint;
    }
}
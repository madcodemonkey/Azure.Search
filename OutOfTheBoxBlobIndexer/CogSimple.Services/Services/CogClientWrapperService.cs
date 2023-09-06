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
    private SearchClient? _searchClient;

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
        if (_searchClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.SearchKey);
            _searchClient = new SearchClient(serviceEndpoint, indexName, credential);
        }

        return _searchClient;
    }

    private Uri GetServiceEndpoint()
    {
        Uri serviceEndpoint = new Uri(_settings.SearchEndpoint);
        return serviceEndpoint;
    }
}
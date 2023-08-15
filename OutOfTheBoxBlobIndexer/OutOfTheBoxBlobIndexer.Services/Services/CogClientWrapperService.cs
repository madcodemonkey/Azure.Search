using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace OutOfTheBoxBlobIndexer.Services;

public class CogClientWrapperService : ICogClientWrapperService
{
    private readonly CogClientSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchIndexerClient? _indexerClient;
    private SearchClient? _searchClient;
    /// <summary>
    /// Constructor
    /// </summary>
    public CogClientWrapperService(CogClientSettings settings)
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

    public SearchIndexerClient GetIndexerClient()
    {
        if (_indexerClient == null)
        {
            var serviceEndpoint = GetServiceEndpoint();
            var credential = new AzureKeyCredential(_settings.CognitiveSearchKey);

            _indexerClient = new SearchIndexerClient(serviceEndpoint, credential);
        }

        return _indexerClient;
    }
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
        Uri serviceEndpoint = new Uri(_settings.CognitiveSearchEndpoint);
        return serviceEndpoint;
    }
}
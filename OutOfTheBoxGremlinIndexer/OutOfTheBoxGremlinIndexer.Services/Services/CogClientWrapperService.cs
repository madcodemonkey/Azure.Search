using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace CustomSqlServerIndexer.Services;

public class CogClientWrapperService : ICogClientWrapperService
{
    private readonly ServiceSettings _settings;
    private SearchIndexClient? _indexClient;
    private SearchIndexerClient? _indexerClient;
    private SearchClient? _searchClient;
    /// <summary>
    /// Constructor
    /// </summary>
    public CogClientWrapperService(ServiceSettings settings)
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
        Uri serviceEndpoint = new Uri(_settings.CognitiveSearchEndpoint);
        return serviceEndpoint;
    }
}
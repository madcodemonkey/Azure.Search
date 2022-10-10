using System.Net;
using Azure;
using Azure.Search.Documents.Indexes;

namespace Search.Services;

public class SearchIndexerService : ISearchIndexerService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexerClient? _client;
    //  private readonly SearchClientOptions _clientOptions;

    /// <summary>Constructor</summary>
    public SearchIndexerService(SearchServiceSettings settings)
    {
        _settings = settings;
        //    _clientOptions =  CreateSearchClientOptions();
    }

    public SearchIndexerClient ClientIndexer => _client ??= new SearchIndexerClient(
        new Uri(_settings.SearchEndPoint), new AzureKeyCredential(_settings.SearchApiKey));


    /// <summary>Gets a list of data sources</summary>
    /// <param name="indexerName">The name of the indexer</param>
    public async Task<bool> DeleteIndexerAsync(string indexerName)
    {
        var response = await ClientIndexer.DeleteIndexerAsync(indexerName);
        
        if (response != null && response.Status == (int) HttpStatusCode.NoContent) //  204 is No Content
        {
            return true;
        }

        return false;
    }


    /// <summary>Gets a list of indexers</summary>
    public async Task<List<string>> GetIndexerListAsync()
    {
        Response<IReadOnlyList<string>> response = await ClientIndexer.GetIndexerNamesAsync();

        List<string> result = response.Value.ToList();

        return result;
    }

    public async Task RunIndexerAsync(string indexerName)
    {
        await ClientIndexer.RunIndexerAsync(indexerName);
    }
}
using System.Net;
using Azure;
using Azure.Search.Documents.Indexes;

namespace Search.Services;

public class SearchIndexerService : ISearchIndexerService
{
    protected SearchServiceSettings Settings { get; }
    private SearchIndexerClient? _client;

    /// <summary>Constructor</summary>
    public SearchIndexerService(SearchServiceSettings settings)
    {
        Settings = settings;
    }

    public SearchIndexerClient ClientIndexer => _client ??= new SearchIndexerClient(
        new Uri(Settings.SearchEndPoint), new AzureKeyCredential(Settings.SearchApiKey));


    /// <summary>Deletes the named indexer</summary>
    /// <param name="indexerName">The name of the indexer to delete</param>
    public async Task<bool> DeleteAsync(string indexerName)
    {
        var response = await ClientIndexer.DeleteIndexerAsync(indexerName);
        
        if (response != null && response.Status == (int) HttpStatusCode.NoContent) //  204 is No Content
        {
            return true;
        }

        return false;
    }


    /// <summary>Gets a list of indexers</summary>
    public async Task<List<string>> GetListAsync()
    {
        Response<IReadOnlyList<string>> response = await ClientIndexer.GetIndexerNamesAsync();

        List<string> result = response.Value.ToList();

        return result;
    }

    /// <summary>Runs an indexer now.</summary>
    /// <param name="indexerName">Name of the indexer to run</param>
    public async Task RunAsync(string indexerName)
    {
        await ClientIndexer.RunIndexerAsync(indexerName);
    }
}
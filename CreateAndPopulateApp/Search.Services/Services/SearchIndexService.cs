using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Search.Services;

public class SearchIndexService : ISearchIndexService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexClient? _client;

    public SearchIndexService(SearchServiceSettings settings)
    {
        _settings = settings;
    }

    public SearchIndexClient Client => _client ??= new SearchIndexClient(new Uri(_settings.SearchEndPoint), new AzureKeyCredential(_settings.SearchAdminApiKey));

    public async Task<bool> CreateIndexAsync<T>(T typeToCreate, string indexName)
    {
        if (await Client.GetIndexAsync(indexName) != null)
            return false; // it already exists

        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(T));
        var searchIndex = new SearchIndex(indexName, searchFields);

        Response<SearchIndex>? result = await Client.CreateOrUpdateIndexAsync(searchIndex);

        return result != null && result.Value != null;
    }

    public async Task<bool> DeleteIndexAsync(string indexName)
    {
        if (await Client.GetIndexAsync(indexName) != null)
            return true; // it did not exists

        await Client.DeleteIndexAsync(indexName);

        return true;
    }

}
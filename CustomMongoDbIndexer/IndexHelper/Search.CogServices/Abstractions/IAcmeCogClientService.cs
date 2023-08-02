using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace Search.CogServices;

public interface IAcmeCogClientService
{
    /// <summary>
    /// Get the client that is used to manipulate the index.
    /// </summary>
    SearchIndexClient GetIndexClient();

    /// <summary>
    /// Get the client that is used to search the index.
    /// </summary>
    SearchClient GetSearchClient(string indexName);
}
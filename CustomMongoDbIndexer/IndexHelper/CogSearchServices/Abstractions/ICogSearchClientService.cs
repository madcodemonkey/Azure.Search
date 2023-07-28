using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace CogSearchServices.Services;

public interface ICogSearchClientService
{
    /// <summary>
    /// Get the client that is used to manipulate the index.
    /// </summary>
    SearchIndexClient GetIndexClient();

    /// <summary>
    /// Get the client that is used to search the index.
    /// </summary>
    SearchClient GetSearchClient();
}
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace SearchServices.Services;

public interface ISearchClientService
{
    SearchIndexClient GetIndexClient();
    SearchClient GetSearchClient();
}
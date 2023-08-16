using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace CogSimple.Services;

public interface ICogClientWrapperService
{
    SearchIndexClient GetIndexClient();
    SearchIndexerClient GetIndexerClient();

    SearchClient GetSearchClient(string indexName);
}
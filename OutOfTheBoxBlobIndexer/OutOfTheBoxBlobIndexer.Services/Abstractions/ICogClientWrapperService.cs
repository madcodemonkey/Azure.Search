using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace OutOfTheBoxBlobIndexer.Services;

public interface ICogClientWrapperService
{
    SearchIndexClient GetIndexClient();

    public SearchIndexerClient GetIndexerClient();

    SearchClient GetSearchClient();
}
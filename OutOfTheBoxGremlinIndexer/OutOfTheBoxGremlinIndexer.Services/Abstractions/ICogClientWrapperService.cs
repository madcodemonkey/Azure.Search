using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace CustomSqlServerIndexer.Services;

public interface ICogClientWrapperService
{
    SearchIndexClient GetIndexClient();

    public SearchIndexerClient GetIndexerClient();

    SearchClient GetSearchClient();
}
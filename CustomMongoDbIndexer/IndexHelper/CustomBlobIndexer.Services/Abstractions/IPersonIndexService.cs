using IndexHelper.Models;
using SearchServices.Services;

namespace IndexHelper.Services;

public interface IPersonIndexService : ISearchIndexService
{
    /// <summary>
    /// Create index or update the index.
    /// </summary>
    void CreateOrUpdateIndex();
   
    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    void UploadDocuments(SearchIndexDocument doc);
}
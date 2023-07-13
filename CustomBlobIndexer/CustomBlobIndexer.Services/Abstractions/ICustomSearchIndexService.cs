using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomSearchIndexService
{
    /// <summary>
    /// Create index or update the index.
    /// </summary>
    void CreateOrUpdateIndex();

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    void UploadDocuments(Document doc);
}
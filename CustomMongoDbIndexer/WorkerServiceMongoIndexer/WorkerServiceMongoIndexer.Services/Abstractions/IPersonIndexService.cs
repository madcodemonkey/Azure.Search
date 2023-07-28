using CogSearchServices.Services;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public interface IPersonIndexService : ICogSearchIndexService
{
  
    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    void UploadDocuments(PersonIndexDocument doc);
}
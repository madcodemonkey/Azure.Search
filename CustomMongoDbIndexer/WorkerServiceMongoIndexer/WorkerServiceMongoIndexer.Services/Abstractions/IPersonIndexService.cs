using CogSearchServices.Services;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public interface IPersonIndexService : ICogSearchIndexService
{
    Task UpdateDocumentAsync(PersonIndexDocument personDoc);
    Task CreateDocumentAsync(PersonIndexDocument personDoc);
    Task DeleteDocumentAsync(string id);
}
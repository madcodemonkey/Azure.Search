using Search.CogServices;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public interface IPersonIndexService : IAcmeCogIndexService
{
    Task UpdateDocumentAsync(PersonIndexDocument personDoc);
    Task CreateDocumentAsync(PersonIndexDocument personDoc);
    Task DeleteDocumentAsync(string id);
}
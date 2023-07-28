using MongoDB.Bson;

namespace WorkerServiceMongoIndexer.Services;

public interface IMongoDocumentProcessorService
{
    Task UpdateDocumentAsync(BsonDocument? document);
    Task CreateDocumentAsync(BsonDocument? document);
    Task DeleteDocumentAsync(string? id);
}
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public class MongoDocumentProcessorService : IMongoDocumentProcessorService
{
    private readonly ILogger<MongoDocumentProcessorService> _logger;
    private readonly IPersonIndexService _personIndexService;

    public MongoDocumentProcessorService(ILogger<MongoDocumentProcessorService> logger,
        IPersonIndexService personIndexService)
    {
        _logger = logger;
        _personIndexService = personIndexService;
    }
    public async Task UpdateDocumentAsync(BsonDocument? document)
    {
        if (document != null)
        {
            var personIndexDoc = MapDocument(document);
            await _personIndexService.UpdateDocumentAsync(personIndexDoc);
        }
        else
        {
            _logger.LogWarning("Document was null for Update!!");
        }
    }


    public async Task CreateDocumentAsync(BsonDocument? document)
    {
        if (document != null)
        {
            var personIndexDoc = MapDocument(document);
            await _personIndexService.CreateDocumentAsync(personIndexDoc);
        }
        else
        {
            _logger.LogWarning("Document was null for create!!");
        }

    }

    public async Task DeleteDocumentAsync(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.LogWarning("Delete was sent a null or empty id!!");
            
        }
        else
        {
            await _personIndexService.DeleteDocumentAsync(id);
            _logger.LogInformation($"Delete this id: {id}");
        }
    }

    private static PersonIndexDocument MapDocument(BsonDocument document)
    {
        return new PersonIndexDocument
        {
            Id = document["_id"].ToString() ?? string.Empty,
            FirstName = document.GetValue(nameof(PersonIndexDocument.FirstName)).AsString,
            LastName = document.GetValue(nameof(PersonIndexDocument.LastName)).AsString,
            Age = document.GetValue(nameof(PersonIndexDocument.Age)).AsInt32,
            Description = document.GetValue(nameof(PersonIndexDocument.Description)).AsString
        };
    }
}
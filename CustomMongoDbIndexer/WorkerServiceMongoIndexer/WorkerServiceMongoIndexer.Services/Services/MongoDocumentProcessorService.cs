using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Text;

namespace WorkerServiceMongoIndexer.Services;

public class MongoDocumentProcessorService : IMongoDocumentProcessorService
{
    private readonly ILogger<MongoDocumentProcessorService> _logger;

    public MongoDocumentProcessorService(ILogger<MongoDocumentProcessorService> logger)
    {
        _logger = logger;
    }
    public async Task UpdateDocumentAsync(BsonDocument? document)
    {
        if (document != null)
        {
            var sb = new StringBuilder();
            // Show all the fields on the document.
            foreach (string name in document.Names)
            {
                sb.AppendLine($"  {name}: {document[name]}");
            }

            _logger.LogInformation(sb.ToString());
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
            var sb = new StringBuilder();
            // Show all the fields on the document.
            foreach (string name in document.Names)
            {
                sb.AppendLine($"  {name}: {document[name]}");
            }

            _logger.LogInformation(sb.ToString());
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
            _logger.LogInformation($"Delete this id: {id}");
        }
    }
}
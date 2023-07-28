using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBServices;

namespace WorkerServiceMongoIndexer.Services;

public class MonitorDatabaseService : IMonitorDatabaseService
{
    private readonly ILogger<MonitorDatabaseService> _logger;
    private readonly IndexerAppSettings _settings;
    private readonly IMongoClientService _clientService;
    private readonly IMongoResumeTokenService _resumeTokenService;
    private readonly IMongoDocumentProcessorService _documentProcessorService;

    /// <summary>
    /// Constructor
    /// </summary>
    public MonitorDatabaseService(ILogger<MonitorDatabaseService> logger,
        IndexerAppSettings settings, 
        IMongoClientService clientService,
        IMongoResumeTokenService resumeTokenService,
        IMongoDocumentProcessorService documentProcessorService)
    {
        _logger = logger;
        _settings = settings;
        _clientService = clientService;
        _resumeTokenService = resumeTokenService;
        _documentProcessorService = documentProcessorService;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var resumeToken = _resumeTokenService.LoadResumeToken();

        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.Default,
            ResumeAfter = string.IsNullOrWhiteSpace(resumeToken) ?
                null : new BsonDocument().Add("_data", resumeToken)
        };
        
        IChangeStreamCursor<ChangeStreamDocument<BsonDocument>> streamCursor = await _clientService.Client
            .GetDatabase(_settings.MongoDatabaseName)
            .WatchAsync(options, cancellationToken);

        foreach (ChangeStreamDocument<BsonDocument> changeItem in streamCursor.ToEnumerable())
        {
            _logger.LogInformation($"Key that changed: {changeItem.DocumentKey}  Operation Type: {changeItem.OperationType}");

            switch (changeItem.OperationType)
            {
                case ChangeStreamOperationType.Create:
                case ChangeStreamOperationType.Insert:
                    await _documentProcessorService.CreateDocumentAsync(changeItem.FullDocument);
                    break;
                case ChangeStreamOperationType.Modify:
                case ChangeStreamOperationType.Update:
                case ChangeStreamOperationType.Replace:
                    await _documentProcessorService.UpdateDocumentAsync(changeItem.FullDocument);
                    break;
                case ChangeStreamOperationType.Delete:
                    await _documentProcessorService.DeleteDocumentAsync(changeItem.DocumentKey.ToString());
                    break;
            }

            var currentResumeToken = changeItem.ResumeToken["_data"];
            _resumeTokenService.SaveResumeToken(currentResumeToken.ToString());

            if (cancellationToken.IsCancellationRequested)
                break;
        }
    }
}
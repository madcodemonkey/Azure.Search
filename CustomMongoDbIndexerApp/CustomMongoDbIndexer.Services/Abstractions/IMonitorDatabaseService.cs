namespace CustomMongoDbIndexer.Services;

public interface IMonitorDatabaseService
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
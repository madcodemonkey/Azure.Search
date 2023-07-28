namespace WorkerServiceMongoIndexer.Services;

public interface IMonitorDatabaseService
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
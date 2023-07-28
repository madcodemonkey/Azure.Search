namespace WorkerServiceMongoIndexer.Services;

public class IndexerAppSettings
{
    public string MongoAtlasConnectionString { get; set; }
    public string MongoAtlasNameOfDatabaseToMonitor { get; set; }
}
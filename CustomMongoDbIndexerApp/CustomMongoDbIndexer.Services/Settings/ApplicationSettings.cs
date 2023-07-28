namespace CustomMongoDbIndexer.Services;

public class ApplicationSettings
{
    public string MongoAtlasConnectionString { get; set; }
    public string MongoAtlasNameOfDatabaseToMonitor { get; set; }
}
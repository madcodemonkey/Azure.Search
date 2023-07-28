using MongoDB.Driver;

namespace WorkerServiceMongoIndexer.Services;

public class MongoClientService : IMongoClientService
{
    private IMongoClient? _client;
    private readonly IndexerAppSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public MongoClientService(IndexerAppSettings settings)
    {
        _settings = settings;
    }
    
    /// <summary>
    /// A Mongo client
    /// </summary>
    public IMongoClient Client => _client ??= new MongoClient(_settings.MongoAtlasConnectionString);

    public IMongoDatabase GetDatabase(string databaseName)
    {
        var mongoDatabase = Client.GetDatabase(databaseName);
        return mongoDatabase;
    }

    public IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
    {
        var db = GetDatabase(databaseName);
        var mongoCollection = db.GetCollection<T>(collectionName);
        return mongoCollection;
    }
}
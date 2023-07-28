using MongoDB.Driver;

namespace CustomMongoDbIndexer.Services;

public interface IMongoClientService
{
    /// <summary>
    /// A Mongo client
    /// </summary>
    IMongoClient Client { get; }

    IMongoDatabase GetDatabase(string databaseName);
    IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName);
}
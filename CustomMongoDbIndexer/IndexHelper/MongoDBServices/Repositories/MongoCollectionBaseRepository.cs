using MongoDB.Driver;

namespace MongoDBServices;

public abstract class MongoCollectionBaseRepository<T> where T : MongoEntity
{
    private readonly IMongoClientService _clientService;

    /// <summary>
    /// Constructor
    /// </summary>
    protected MongoCollectionBaseRepository(IMongoClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task CreateAsync(T mongoEntity)
    {
        var collection = GetCollection();
        await collection.InsertOneAsync(mongoEntity);
    }

    public async Task DeleteAsync(T mongoEntity)
    {
        await DeleteAsync(mongoEntity.Id);
    }

    public async Task DeleteAsync(string id)
    {
        var collection = GetCollection();
        await collection.DeleteOneAsync(c => c.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        var collection = GetCollection();
        var results = await collection.FindAsync(_ => true);
        return await results.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var collection = GetCollection();
        var filter = Builders<T>.Filter.Eq(nameof(MongoEntity.Id), id);
        var result = await collection.FindAsync(filter, new FindOptions<T>() { Limit = 1 });
        var oneItem = await result.FirstOrDefaultAsync();
        return oneItem;
    }

    public async Task UpdateAsync(T mongoEntity)
    {
        var collection = GetCollection();
        var filter = Builders<T>.Filter.Eq(nameof(MongoEntity.Id), mongoEntity.Id);
        await collection.ReplaceOneAsync(filter, mongoEntity, new ReplaceOptions { IsUpsert = true });
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var collection = GetCollection();
        long numberOfHits = await collection.CountDocumentsAsync(w => w.Id == id, new CountOptions { Limit = 1 });
        return numberOfHits > 0;
    }

    private IMongoCollection<T> GetCollection()
    {
        return _clientService.GetCollection<T>(DatabaseName, CollectionName);
    }

    protected abstract string DatabaseName { get; }
    protected abstract string CollectionName { get; }
}
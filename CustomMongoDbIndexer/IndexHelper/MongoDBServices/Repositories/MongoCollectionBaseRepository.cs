using MongoDB.Driver;

namespace MongoDBServices;

public abstract class MongoCollectionBaseRepository<T> : IMongoCollectionRepository<T> where T : MongoEntity
{
    private readonly IMongoClientService _clientService;

    /// <summary>
    /// Constructor
    /// </summary>
    protected MongoCollectionBaseRepository(IMongoClientService clientService)
    {
        _clientService = clientService;
    }

    /// <summary>
    /// Collection name
    /// </summary>
    protected abstract string CollectionName { get; }

    /// <summary>
    /// The database name.
    /// </summary>
    protected abstract string DatabaseName { get; }

    /// <summary>
    /// Creates a document
    /// </summary>
    /// <param name="mongoEntity">The document to create</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task CreateAsync(T mongoEntity, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        await collection.InsertOneAsync(mongoEntity, null, cancellationToken);
    }

    /// <summary>
    /// Deletes one document
    /// </summary>
    /// <param name="mongoEntity">The document that you want to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task DeleteAsync(T mongoEntity, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(mongoEntity.Id, cancellationToken);
    }


    /// <summary>
    /// Deletes one document by id
    /// </summary>
    /// <param name="id">The id of the document to delete.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        await collection.DeleteOneAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Checks to see if a document exists
    /// </summary>
    /// <param name="id">The id of the document</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        long numberOfHits = await collection.CountDocumentsAsync(w => w.Id == id, new CountOptions { Limit = 1 }, cancellationToken);
        return numberOfHits > 0;
    }

    /// <summary>
    /// Gets every document in the database.  If there are lots, You should use the paging overload of this method!
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        var results = await collection.FindAsync(_ => true, null, cancellationToken);
        return await results.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets items by the page.
    /// </summary>
    /// <param name="pageNumber">The one based paged number (should be greater than zero).</param>
    /// <param name="itemsPerPage">Number of items per page (should be greater than zero).</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<MongoPage<T>> GetAllAsync(int pageNumber, int itemsPerPage, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) 
            pageNumber = 1;
        if (itemsPerPage < 1)
            itemsPerPage = 1;

        int skipNumber = (pageNumber - 1) * itemsPerPage;
        
        var collection = GetCollection();
        var query = collection.Find(_ => true);
        var totalTask = query.CountDocumentsAsync(cancellationToken);
        var itemsTask = query.Skip(skipNumber).Limit(itemsPerPage).ToListAsync(cancellationToken);
        await Task.WhenAll(totalTask, itemsTask);
        return new MongoPage<T>() { PageNumber = pageNumber, Items = itemsTask.Result, Total = totalTask.Result };
    }

    /// <summary>
    /// Gets a document by its id.
    /// </summary>
    /// <param name="id">The id of the document to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        var filter = Builders<T>.Filter.Eq(nameof(MongoEntity.Id), id);
        var result = await collection.FindAsync(filter, new FindOptions<T>() { Limit = 1 }, cancellationToken);
        var oneItem = await result.FirstOrDefaultAsync(cancellationToken);
        return oneItem;
    }

    /// <summary>
    /// Updates one document.  It will also create the document if it does not exist.
    /// </summary>
    /// <param name="mongoEntity">The document to update.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task UpdateAsync(T mongoEntity, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection();
        var filter = Builders<T>.Filter.Eq(nameof(MongoEntity.Id), mongoEntity.Id);
        await collection.ReplaceOneAsync(filter, mongoEntity, new ReplaceOptions { IsUpsert = true }, cancellationToken);
    }

    /// <summary>
    /// Creates the collection using the database and collection names.
    /// </summary>
    private IMongoCollection<T> GetCollection()
    {
        return _clientService.GetCollection<T>(DatabaseName, CollectionName);
    }
}
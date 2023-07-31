namespace MongoDBServices;

public interface IMongoCollectionRepository<T> where T : MongoEntity
{
    /// <summary>
    /// Creates a document
    /// </summary>
    /// <param name="mongoEntity">The document to create</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task CreateAsync(T mongoEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes one document
    /// </summary>
    /// <param name="mongoEntity">The document that you want to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task DeleteAsync(T mongoEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes one document by id
    /// </summary>
    /// <param name="id">The id of the document to delete.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks to see if a document exists
    /// </summary>
    /// <param name="id">The id of the document</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets every document in the database.  If there are lots, You should use the paging overload of this method!
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets items by the page.
    /// </summary>
    /// <param name="pageNumber">The one based paged number (should be greater than zero).</param>
    /// <param name="itemsPerPage">Number of items per page (should be greater than zero).</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<MongoPage<T>> GetAllAsync(int pageNumber, int itemsPerPage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a document by its id.
    /// </summary>
    /// <param name="id">The id of the document to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates one document.  It will also create the document if it does not exist.
    /// </summary>
    /// <param name="mongoEntity">The document to update.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task UpdateAsync(T mongoEntity, CancellationToken cancellationToken = default);
}
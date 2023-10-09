namespace CustomBlobIndexer.Services;

public interface ICustomIndexService
{
    /// <summary>
    /// Create the index, data source and indexer for an out-of-the-box blob solution.
    /// </summary>
    Task CreateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the index, data source and indexer for the out-of-the-box blob solution.
    /// </summary>
    Task DeleteAsync(CancellationToken cancellationToken = default);
}
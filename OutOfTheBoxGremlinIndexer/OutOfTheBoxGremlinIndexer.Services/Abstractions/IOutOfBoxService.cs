namespace OutOfTheBoxGremlinIndexer.Services;

public interface IOutOfBoxService
{
    /// <summary>
    /// Creates the out-of-the-box data source, indexer and index.
    /// </summary>
    Task CreateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the index, data source and indexer for the out-of-the-box blob solution.
    /// </summary>
    Task DeleteAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// Runs the indexer
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task RunIndexerAsync(CancellationToken cancellationToken = default);
}
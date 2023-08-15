namespace OutOfTheBoxBlobIndexer.Services.Services;

public interface IOutOfBoxService
{
    /// <summary>
    /// Creates the out-of-the-box data source, indexer and index.
    /// </summary>
    Task CreateAsync();

 
    /// <summary>
    /// Runs the indexer
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task RunIndexerAsync(CancellationToken cancellationToken = default);
}
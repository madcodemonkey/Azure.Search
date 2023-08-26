namespace CogSimple.Services;

public interface ICogSearchIndexerService
{
    /// <summary>
    /// Indicates if an indexer exists or not
    /// </summary>
    /// <param name="indexerName">The name of the indexer</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> IndexerExistsAsync(string indexerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an indexer
    /// </summary>
    /// <param name="indexerName">The name of the indexer</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the indexer exists before attempting to delete it.  If you try
    /// to delete an indexer that doesn't exist, it will generate an exception.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task DeleteIndexerAsync(string indexerName, bool checkIfExistsFirst, CancellationToken cancellationToken = default);
}
namespace CustomSqlServerIndexer.Services;

public interface ICustomSqlServerIndexerService
{
    /// <summary>
    /// Does the work necessary when changes are found in the database.
    /// </summary>
    /// <param name="retrievalLimit">The maximum number of records to retrieve</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The number of changes made to the index.</returns>
    Task<int> DoWorkAsync(int retrievalLimit = Int32.MaxValue, CancellationToken cancellationToken = default);
}
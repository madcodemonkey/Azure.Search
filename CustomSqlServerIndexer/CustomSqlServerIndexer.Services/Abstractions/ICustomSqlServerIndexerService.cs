namespace CustomSqlServerIndexer.Services;

public interface ICustomSqlServerIndexerService
{
    /// <summary>
    /// Does the work necessary when changes are found in the database.
    /// </summary>
    /// <returns>The number of changes made to the index.</returns>
    Task<int> DoWorkAsync(CancellationToken cancellationToken = default);
}
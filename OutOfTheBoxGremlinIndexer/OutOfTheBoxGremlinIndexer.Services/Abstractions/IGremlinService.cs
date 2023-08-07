namespace CustomSqlServerIndexer.Services;

public interface IGremlinService 
{

    /// <summary>
    /// Create the out-of-the-box Apache Gremlin indexER.
    /// </summary>
    Task CreateAsync();
}
namespace CustomSqlServerIndexer.Services;

public interface IGremlinCogSearchService 
{

    /// <summary>
    /// Create the out-of-the-box Apache Gremlin indexER.
    /// </summary>
    Task CreateAsync();
}
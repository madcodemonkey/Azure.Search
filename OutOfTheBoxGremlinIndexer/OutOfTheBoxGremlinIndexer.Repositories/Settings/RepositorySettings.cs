namespace CustomSqlServerIndexer.Services;

/// <summary>Settings used by the repository library.</summary>
public class RepositorySettings
{
    /// <summary>
    /// The host name minus any https prefix (e.g., mystuff.gremlin.cosmos.azure.com)
    /// </summary>
    public string GremlinHostName { get; set; } = string.Empty;
    public string GremlinKey { get; set; } = string.Empty;
    public string GremlinDatabaseName { get; set; } = string.Empty;
    public string GremlinContainerName { get; set; } = string.Empty;

}
 
namespace CustomSqlServerIndexer.Repositories;

/// <summary>Settings used by the repository library.</summary>
public class RepositorySettings
{
    public string GremlinContainerName { get; set; } = string.Empty;
    /// <summary>
    /// Gremlin connection string. 
    /// Warning!!! The one from the portal is missing the database name so you have to add that by hand.
    /// Example: { "connectionString" : "AccountEndpoint=https://[Cosmos DB account name].documents.azure.com;AccountKey=[Cosmos DB auth key];Database=[Cosmos DB database id];ApiKind=Gremlin" }
    /// See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#supported-credentials-and-connection-strings
    /// </summary>
    public string GremlinDatabaseConnectionString { get; set; } = string.Empty;

    public string GremlinDatabaseName { get; set; } = string.Empty;
   

    public bool GremlinEnableSSL { get; set; } = true;

    /// <summary>
    /// The host name minus any https prefix (e.g., mystuff.gremlin.cosmos.azure.com)
    /// </summary>
    public string GremlinHostName { get; set; } = string.Empty;
    public int GremlinHostPort { get; set; } = 443;
    public string GremlinKey { get; set; } = string.Empty;
    public string GremlinQueryType { get; set; } = "Vertices";  // Vertices or Edges
    /// <summary>
    /// The name of the soft delete column (optional)
    /// </summary>
    public string? GremlinSoftDeleteColumnName { get; set; }

    /// <summary>
    /// The value that the soft delete column should be changed to in order to indicate a soft delete has occurred.
    /// </summary>
    public string? GremlinSoftDeleteColumnValue { get; set; }
}
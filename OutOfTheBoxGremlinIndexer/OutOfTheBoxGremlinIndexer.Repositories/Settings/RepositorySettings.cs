namespace CustomSqlServerIndexer.Repositories;

/// <summary>Settings used by the repository library.</summary>
public class RepositorySettings
{
    public string GremlinContainerName { get; set; } = string.Empty;
    /// <summary>
    /// Gremlin connection string. 
    /// Warning!!! The one from the portal is missing the database name so you have to add that by hand.
    /// Example: { "connectionString" : "AccountEndpoint=https://[Cosmos DB account name].documents.azure.com;AccountKey=[Cosmos DB auth key];Database=[Cosmos DB database id];ApiKind=MongoDb" }
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
}
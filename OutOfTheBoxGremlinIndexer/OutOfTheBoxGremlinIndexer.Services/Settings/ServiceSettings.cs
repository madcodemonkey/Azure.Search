namespace CustomSqlServerIndexer.Services;
 
public class ServiceSettings
{

    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string CognitiveSearchEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; } = string.Empty;

    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string CognitiveSearchKey { get; set; } = string.Empty;
    /// <summary>
    /// When saving data to the Cognitive Search index, this is the maximum batch size.
    /// </summary>
    public int CognitiveSearchMaxUpsertBatchSize { get; set; }

    /// <summary>
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;

    /// <summary>
    /// Gremlin connection string. 
    /// Warning!!! The one from the portal is missing the database name so you have to add that by hand.
    /// Example: { "connectionString" : "AccountEndpoint=https://[Cosmos DB account name].documents.azure.com;AccountKey=[Cosmos DB auth key];Database=[Cosmos DB database id];ApiKind=MongoDb" }
    /// See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#supported-credentials-and-connection-strings
    /// </summary>
    public string GremlinDatabaseConnectionString { get; set; } = string.Empty;
}
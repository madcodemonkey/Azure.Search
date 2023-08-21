# Azure Cognitive Search Out-of-the-box indexer for Cosmos Apache Gremlin

This project is designed to do three things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Help you create the data in the Gremlin database so that the out-of-the-box indexer picks them up and then you can query.
3. Give you a very basic azure cogntive search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection to help you do a POST to this endpoint.

# Gremlin .NET NuGet package notes
- For Cosmos DB Apache Gremlin, the current recommended version is 3.4.13 ([reference the compatible client libraries here](https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/support#compatible-client-libraries))
- See also [Documenation on the tinkerpop web site](https://tinkerpop.apache.org/docs/3.2.7/reference/#gremlin-DotNet)
- Only use primitive types for properties.  Do not use complex types in Cosmos DB Apache Gremlin 
- [Documenatation recommends passing the traversal as a string](https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/support#unsupported-features); however we can use Grelmin.Net.Extensions for parameterized queries to lessen injection fears.
   - Quote from the docs:  Gremlin Bytecode is a programming language agnostic specification for graph traversals. Azure Cosmos DB Graph doesn't support it yet. 
     Use GremlinClient.SubmitAsync() and pass traversal as a text string.

# Gremlin 
- We see the pk value because we are using a flatgraph (as opposed to a partition graph)

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
   - If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.
2. In the Azure portal, create a Cosmos DB and select the "Azure Cosmos DB Apache Gremlin" option 
   - 2a - Cosmos settings
      - Account name: Your choice
      - Location: Your choice
      - Capactity mode: Provisioned throughput
      - Uncheck "Limit the total amount of throughput that can be provisioned on this account"
      - Take defaults for other tabs.
   - 2b - Using the Data Explorer, create a database using the "New Graph" dropdown
      - Database id: sample-database        (if you use a different name update GremlinDatabaseName in the local.settings.json too!)
      - Database througput: Manual
      - Estimated your required RUs:  400
   - 2c - Using the Data Explorer, create a graph container using the "New Graph" dropdown
      - Database id:  Use existing and select
      - Graph id: sample-graph             (if you use a different name update GremlinContainerName in the local.settings.json too!)
      - Partition key: /pk
   - Note: Heavily based on this quickstart: https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/quickstart-dotnet?tabs=windows
3. Update the local.settings.json entries with information from the portal
   - Cognitive Search information to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchEndpoint - This is the URL of your cognitive search endpoint (e.g., https://[name-you-choose].search.windows.net)
   -  Cosmos Apache Gremlin information to update
      - GremlinHostName - In the Azure Portal under Settings/Keys - This is the host name minus any https prefix (e.g., mystuff.gremlin.cosmos.azure.com).  The GREMLIN URI has most of it for you to copy and paste (just remove the wss:// prefix and the :443/ suffix).
      - GremlinKey - In the Azure Portal under Settings/Keys - this is the primary or secondary key 
      - GremlinDatabaseConnectionString - In the Azure Portal under Settings/Keys - this is the primary or secondary connection string
         - Warning!!! The one from the portal is missing the database name so you have to add that by hand.
         - Example: { "GremlinDatabaseConnectionString" : "AccountEndpoint=https://[Cosmos DB account name].documents.azure.com:443/;AccountKey=[Cosmos DB auth key];Database=sample-database;ApiKind=MongoDb" }
         - See docs: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin#supported-credentials-and-connection-strings

# Running
1. Run the "OutOfTheBoxGremlinIndexer" function project.  
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. Using the postman collection, do a POST using the "Gremlin DATA - Create all" request
4. Using the postman collection, do a GET using the "Gremlin Get People" request to make sure the data is in the index.
5. Using the postman collection, force the indexer to run bo doing a GET to the "IndexER run" request.
6. Use the postman to do searches against the search endpoint using the "Cognitive Search-simple" request.

# Documentation
- Based loosely on this example: https://learn.microsoft.com/en-us/azure/search/search-howto-index-cosmosdb-gremlin
- Cosmos DB creation based heavily on: https://learn.microsoft.com/en-us/azure/cosmos-db/gremlin/quickstart-dotnet?tabs=windows
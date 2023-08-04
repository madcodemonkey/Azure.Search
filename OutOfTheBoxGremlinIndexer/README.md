# Azure Cognitive Search Out-of-the-box indexer for Cosmos Apache Gremlin

This project is designed to do three things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Help you make changes to the Gremlin database so that the out-of-the-box indexer picks them up and then you can query.
3. Give you a very basic search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection to help you do a POST to this endpoint.

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
   - If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.
2. In the Azure portal, create a Cosmos Apache Gremlin database and container
   - Note 1: Using the Data Explorer, create a database using these settings
      -   
   - Note 2: Using the Data Explorer, create a graph container using these settings
      -  
3. Update the local.settings.json entries with information from the portal
   - Cognitive Search information to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchEndpoint - This is the URL of your cognitive search endpoint (e.g., https://[name-you-choose].search.windows.net)
   -  Cosmos Apache Gremlin information to update
      - GremlinHostName - In the Azure Portal under Settings/Keys - This is the host name minus any https prefix (e.g., mystuff.gremlin.cosmos.azure.com).  The GREMLIN URI has most of it for you to copy and paste (just remove the wss:// prefix and the :443/ suffix).
      - GremlinKey - In the Azure Portal under Settings/Keys - this is the primary or secondary key 

# Running
1. Run the "OutOfTheBoxGremlinIndexer" function project.  
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. Use the postman to do searches against the search endpoint, which is searching the cognitive search index.
4. Use the postman collection to manipulate the gremlin vertices or edges.

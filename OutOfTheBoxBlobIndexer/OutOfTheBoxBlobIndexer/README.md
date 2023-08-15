# Azure Cognitive Search Custom SQL Server Indexer

This project is designed to do three things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Fire a timer periodically OR when triggered manually Trigger to check for database changes (see XXXTimer or YYY HttpTrigger)
3. Give you a very basic search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection to help you do a POST to this endpoint.

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
   - If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.
2. In the Azure portal, create a SQL Server and a SQL Database 
   - Note 1: Under Networking, choose "ADD YOUR CLIENT ipV4 adddress (....)" 
   - Note 2: Under Networking, choose "Selected networks" and then scroll down and check "Allow Azure services and resources to access this server".  
   - Note 3: Save your changes by hitting the "Save" button at the bottom.  
3. Update the local.settings.json entries with information from the portal
   - Cognitive Search information to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchEndpoint - This is the URL of your cognitive search endpoint (e.g., https://[name-you-choose].search.windows.net)
   - Database information to update
      - DatabaseConnectionString - Your database connection string

# Running
EF migrations will run upon start and it can take a number of seconds (up to 60 to get things created).  
After initial creation, to avoid running it everytime, see the DatabaseRunMigrationsOnStartup setting in local.settings.json.

1. Run the "CustomSqlServerIndexer" function project.   It is designed to use EF migrations to create and seed the database.  This can take up to 60 seconds the firs time.
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. Use postman (see postman collection) to do searches against the search endpoint, which is searching the cognitive search index.

# Postman collection
There is a POSTMAN collection that can be used againt the HttpTrigger endpoints.

# Things that could be done better
- In HighWaterMarkStorageService.cs, actually store the high watermark in a database, redis cache or something external.
- In CustomSqlServerIndexerService.cs, there is a TODO to queue errors for reprocessing rather than continuely hitting the over and over again.  
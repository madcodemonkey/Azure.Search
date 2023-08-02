# Azure Cognitive Search Custom SQL Server Indexer

This project is designed to do three things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Fire a timer periodically OR when triggered manually Trigger to check for database changes (see XXXTimer or YYY HttpTrigger)
3. Give you a very basic search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection to help you do a POST to this endpoint.

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
2. TODO: In the Azure portal, create a SQL Database 
3. Update the local.settings.json entries with information from the portal
   - Cognitive Search inforamtion to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchName - This is what you called the cognitive search when you created it in the portal (e.g., mycoolcogsearch).

# Running
1. Run the "CustomSqlServerIndexer" function project.
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. TODO: populuate with data....
4. Use postman (see postman collection) to do searches against the search endpoint, which is searching the cognitive search index.

# Postman collection
There is a POSTMAN collection that can be used againt the HttpTrigger endpoints.
- If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.

# Notes
- 
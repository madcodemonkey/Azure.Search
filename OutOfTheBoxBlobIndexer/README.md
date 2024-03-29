# Azure Cognitive Search Out-Of-The-Box Blob Indexer

This project is designed to do three things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Give you a very basic search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection at the root of this repository to help you do a POST to this endpoint.

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
   - If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.
2. In the Azure portal, create a storage account
   - Create a container called myblobcontainer (or change the name in the local.settings.json file).
   - Upload some files to the container.
3. Update the local.settings.json entries with information from the portal
   - Cognitive Search information to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchEndpoint - This is the URL of your cognitive search endpoint (e.g., https://[name-you-choose].search.windows.net)
   - Storage account information
      - CognitiveSearchStorageConnectionString - A connection string that will allow us to search blob files.

# Running
1. Run the "OutOfTheBoxBlobIndexer" function project. 
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. Use postman (see postman collection) to do searches against the search endpoint, which is searching the cognitive search index.

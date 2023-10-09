# Azure Cognitive Search Custom Blob Indexer

This project is designed to do four things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
2. Trigger when files are uploaded to a blob and put documents in the Cognitive Search index (see BlobTriggerFunction.cs and theBlobTrigger)
3. Give you a very basic search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection at the root of this repository to help you do a POST to this endpoint.
4. Give you a very basic Open AI search that uses Cognitive Search (semantic query type, so it has to be turned on) and then a query to Open AI engine.  It's loosely based on [this Python example](https://github.com/Azure-Samples/azure-search-openai-demo/blob/main/README.md)

You do NOT have to use the Open AI parts of this example nor do you have to use the search endpoint if all you want is an indexer example.  This just allowed me a common project to show a simple implementation of each!

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
2. In the Azure portal, create instance of an Azure Cognitive Services resource (there is currently on one SKU to pick from so there is no choice to make for that).
3. In the Azure portal, create instance of a storage account and create a container within it called my-files
4. Update the local.settings.json entries with information from the portal
   - Storage Account information to update
      - Blob:AccessKey - Get the key for either key1 or key2 from the Azure Portal. See Security + networking section and the "Access Keys" item.
      - Blob:AccountName - This is what you called the storage account when you created it in the portal (e.g., mystorage).
      - Blob:StorageConnectionString -  Get the Connection string for either key1 or key2 from the Azure Portal. See Security + networking section and the "Access Keys" item.
   - Cognitive Search information to update
      - CognitiveSearch:Key - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearch:Endpoint - This is what you called the cognitive search  (e.g., https://xxxx.search.windows.net).
   - Cognitive Services information to update
      - CognitiveService:Endpoint - This is the endpoint for the cognitive service.  See Resource management section and the "Keys and endpoint" item.
      - CognitiveService:Key - Get the key for either key 1 or key 2 from the Azure Portal. See Resource management section and the "Keys and endpoint" item.
5. Optional local.settings.json 
   - AppSetting:ChunkIntoMultipleDocuments - By default it is false, but if you want to make the docs bite size for Open AI, you should set this to true.
   - AppSetting:ChunkMaximumNumberOfCharacters - The default doc size is 1000 when chunking in order to control token usage for Open AI.
   - OpenAI:DeploymentOrModelName - The name of your deployment here.  This comes from the Open AI Studio (look under your deployments)
   - OpenAI:Endpoint - Your API endpoint here from the Azure Portal (see Keys and Endpoint in the Resource Management section)
   - OpenAI:Key - Your key here from the Azure Portal (see Keys and Endpoint in the Resource Management section)

# Running
1. Run the "CustomBlobIndexer" function project.
2. If you haven't created an index, first hit the  IndexCreator: [GET,POST] http://localhost:7071/api/IndexCreator to create the index.  You can use a browser since GET is available.
3. Upload a document to the my-files container to trigger the blob indexer.  This can be done manually via the portal.
4. Use postman (see postman collection at the root of the repository) to do searches against the search endpoint, which is searching the cognitive search index.
5. If you configured Open AI, used the postman collection to do posts against the open ai endpoint; otherwise, avoid it or you will receive errors.

# Postman collection
There is a POSTMAN collection, at the root of the repository, that can be used againt the HttpTrigger endpoints.
- If you plan to do semantic searches, you'll need to turn that on in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.

# Notes
- A blob trigger was used to make this demo easier to understand and play with; however, it's not the best way to trigger on a blob change.  Using Event Grid is a better choice 
  since it is faster (blob trigger can take up to 10 minutes if the function has gone to sleep).
- 

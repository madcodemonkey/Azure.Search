# Azure Cognitive Search Vector example

This project is designed to do four things
1. Create an Azure Cognitive Search index (see IndexManipulationFunction.cs and the IndexCreator HttpTrigger)
   - You'll need to turn on semantic search preview in the Azure Portal.  See the "Semantic search (preview)" option under Settings in your Azure Cognitive Search resource.   
2. Populate data in the index (see DataManipulationFunction.cs and the Data-Creator HttpTrigger endpoint)  
3. Update the embeddings on the vector fields in the index (see DataManipulationFunction.cs and the Data-Update-Embeddings HttpTrigger endpoint)
3. Give you a search endpoint (see IndexSearchFunction.cs and the IndexSearch HttpTrigger).  There is a provided Postman collection at the root of this repository to help you do a POST to this endpoint.

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
2. In the Azure portal, create an Azure OpenAI resource (currently on one SKU)
3. In the Azure AI Studio, create a deployment model using the "text-embedding-ada-002" model.
4. Update the local.settings.json entries with information from the portal
   - Cognitive Search information to update
      - CognitiveSearch:Key - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearch:Endpoint - This is what you called the cognitive search  (e.g., https://xxxx.search.windows.net).
   - OpenAI:DeploymentOrModelName - The name of your deployment here.  This comes from the Open AI Studio (look under your deployments)
   - OpenAI:Endpoint - Your API endpoint here from the Azure Portal (see Keys and Endpoint in the Resource Management section ... e.g., https://xxxxx.openai.azure.com/)
   - OpenAI:Key - Your key here from the Azure Portal (see Keys and Endpoint in the Resource Management section)

# Running
1. Run the "VectorExample" function project.
2. Use the postman collection (see "Create Index" under VectorExample) to first hit IndexCreator: [POST] http://localhost:7071/api/IndexCreator to create the index.
3. Next, use the postman collection (see "Create Data" under VectorExample) to create the documents (the embeddings fields will not be populated yet).
4. Next, use the postman collection (see "Update embeddings" under VectorExample) to update the TitleVector and ContentVector fields in batches.  This can be time
   consuming if you setup your OpenAI deployment model with low capacity, so you might experience timeouts (locally that 30 minutes for Azure Functions by default).  
   If you did timeouts, just call the endpoint again till you is no longer updating docs.  It will save every ten items as it processes the missing vector fields and
   only retrieves documents where VectorEmbeddingVersion is equal to zero.
5. Use the postman collections search examples to do different types of searches (simple, semantic, vector only, and a couple of hybrid methods)

# Postman collection
There is a POSTMAN collection, at the root of the repository, that can be used againt the HttpTrigger endpoints.

# Notes
- This example is based off of the https://github.com/Azure/cognitive-search-vector-pr/tree/main/demo-dotnet  example, but modified to work in a Azure Function.
- 

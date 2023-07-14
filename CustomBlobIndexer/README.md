# Azure Cognitive Search Custom Blob Indexer

# Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will delete an MSDN subscription before the end of the month).
2. In the Azure portal, create instance of an Azure Cognitive Services resource (there is currently on one SKU to pick from so there is no choice to make for that).
3. In the Azure portal, create instance of a storage account and create a container within it called my-files
4. Update the local.settings.json entries with information from the portal
   - Storage Account information to update
      - BlobAccessKey - Get the key for either key1 or key2 from the Azure Portal. See Security + networking section and the "Access Keys" item.
      - BlobAccountName - This is what you called the storage account when you created it in the portal (e.g., mystorage).
      - BlobStorageConnectionString -  Get the Connection string for either key1 or key2 from the Azure Portal. See Security + networking section and the "Access Keys" item.
   - Cognitive Search inforamtion to update
      - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
      - CognitiveSearchName - This is what you called the cognitive search when you created it in the portal (e.g., mycoolcogsearch).
   - Cognitive Services inforamtion to update
      - CognitiveServiceEndpoint - This is the endpoint for the cognitive service.  See Resource management section and the "Keys and endpoint" item.
      - CognitiveServiceKey - Get the key for either key 1 or key 2 from the Azure Portal. See Resource management section and the "Keys and endpoint" item.

# Running
1. Run the "CustomBlobIndexer" function.
2. If you haven't created an index, first hit the  IndexCreatorFunction: [GET,POST] http://localhost:7071/api/IndexCreatorFunction to create the index.
3. Upload a document to the my-files container to trigger the blob indexer.

# Notes
- A blob trigger was used to make this demo easier to understand and play with; however, it's not the best way to trigger on a blob change.  Using Event Grid is a better choice 
  since it is faster (blob trigger can take up to 10 minutes if the function has gone to sleep).
- 

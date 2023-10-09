# MongoDB Custom indexer 
---
# Setup
The IndexHelper and WorkerServiceMongoIndexer are designed to be used together.  
- IndexHelper - It will create the Cogntive Search index and allow you to query it.
- WorkerServiceMongoIndexer - It will monitor the MongoDB database for changes and update the Index created by the IndexHelper.

## Step 1: IndexHelper Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
1. Update the local.settings.json entries with information from the Mongo web site
   - MongoAtlasConnectionString - This is a connection string to an Atlas Mongo DB.  This is needed to monitor the change stream on the database.  This only works with ATLAS MongoDB databases (see comments in the worker below)
1. Update the local.settings.json entries with information from the Azure Portal
   - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - CognitiveSearchEndPoint - The name of your cognitive search instance (e.g., https://[your-service-name].search.windows.net)

## Step 2: WorkerServiceMongoIndexer Setup
1. You'll need a MongoDB Atlas account to run this. 
   - Go to the [start free](https://www.mongodb.com/cloud/atlas/register) page and register for your free account.
   - Note that you can NOT use a Docker container!! According to the MongoDB documentation on [Change Streams](https://www.mongodb.com/basics/change-streams) it requires a MongoDB that:
      - The database must be in a replica set or sharded cluster.
      - The database must use the WiredTiger storage engine.
      - The replica set or sharded cluster must use replica set protocol version 1.
1. Update the appsettings.json entries with information from the Mongo web site
   - MongoAtlasConnectionString - This is a connection string to an Atlas Mongo DB.  This is needed to monitor the change stream on the database.  Again, this only works with ATLAS MongoDB databases (see comment above)
1. Update the appsettings.json entries with information from the Azure portal
   - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - CognitiveSearchEndPoint - The name of your cognitive search instance (e.g., https://[your-service-name].search.windows.net)

# Running
1. **IndexerHelper**. Start the IndexerHelper Azure Function first.
1. **IndexerHelper**. Using the postman collection (at the root of the repository) or a browser, hit the Index-Creator GET endpoint in the IndexManipulationFunction.cs.  This will create the index in Azure Cognitive Search.
1. **WorkerServiceMongoIndexer**. Start the WorkerServiceMongoIndexer.  This will begin monitoring the change stream of the specified database 
1. **IndexerHelper**.  You can now use the postman collection (in same directory as the IndexHelperApp.sln file) to perform CRUD actions against the MongoDB.
   - The Mongo-Create endpoint will create MongoDB records.
   - The Mongo-Delete endpoint will delete MongoDB records.
   - The Mongo-Update endpoint will update MongoDB records.
1. **WorkerServiceMongoIndexer**.  After performing these operations, give the WorkerServiceMongoIndexer project a few seconds to maniuplate the index documents in Azure Cognitive Search index
1. **IndexerHelper**.  Perform searches against the Azure Cognitive Search index using the postman collection
   - The Index-SimpleSearch endpoint will perform simple searches (as opposed to Luncene or Semantic searches) against the Azure Cognitive Search index

If you find the new data in the Azure Cognitive Search index, the worker service is working properly!! 

# Compass Tool
If you want to look into the MongoDB running in the docker container, you can [download MongoDB's free tool called Compass](https://www.mongodb.com/try/download/compass).  

You can get the connection string from the Atlas portal. It will look something like this
```
"mongodb+srv://[userName]:[Password]@[yourclustername].[alphaNumberic].mongodb.net/"
```

# Documenation
- Change Streams: https://www.mongodb.com/basics/change-streams


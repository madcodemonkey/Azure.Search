# MongoDB Custom indexer 
---
# Setup
The IndexHelper and WorkerServiceMongoIndexer are designed to be used together.  
- IndexHelper - It will create the Cogntive Search index and allow you to query it.
- WorkerServiceMongoIndexer - It will monitor the MongoDB database for changes and update the Index created by the IndexHelper.

# IndexHelper Setup
You'll need to do the following
1. In the Azure portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
1. Update the local.settings.json entries with information from the portal
   - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - CognitiveSearchName - This is what you called the cognitive search when you created it in the portal (e.g., mycoolcogsearch).
  
# WorkerServiceMongoIndexer Setup
1. You'll need a MongoDB Atlas account to run this.  Go to the [start free](https://www.mongodb.com/cloud/atlas/register) page and register for your free account.
   - Note: You cannot use a docker container since according to the MongoDB documentation on [Change Streams](https://www.mongodb.com/basics/change-streams) it requires:
      - The database must be in a replica set or sharded cluster.
      - The database must use the WiredTiger storage engine.
      - The replica set or sharded cluster must use replica set protocol version 1.
1. Update the appsettings.json entries with information from the portal
   - MongoAtlasConnectionString
   - MongoDatabaseName
   - CognitiveSearchKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - CognitiveSearchName - This is what you called the cognitive search when you created it in the portal (e.g., mycoolcogsearch).

# Running
You can run the console app called [SimpleCrudExample1](https://github.com/madcodemonkey/MongoDB/tree/main/SimpleCrudExample1) in 
my Mongo repository to update the the data.  Just remember to update the connection string to point to MongoDB Atlas since it pointed
at a local Docker container by default.

# Compass Tool
If you want to look into the MongoDB running in the docker container, you can [download MongoDB's free tool called Compass](https://www.mongodb.com/try/download/compass).  

You can get the connection string from the Atlas portal. It will look something like this
```
"mongodb+srv://[userName]:[Password]@[yourclustername].[alphaNumberic].mongodb.net/"
```

# Documenation
- Change Streams: https://www.mongodb.com/basics/change-streams


MongoDB Custom indexer 

# Setup
You'll need to run a MongoDB Atlas account to run this.  You cannot use a docker container since according to the MongoDB documentation on
[Change Streams](https://www.mongodb.com/basics/change-streams) it requires:
1. The database must be in a replica set or sharded cluster.
1. The database must use the WiredTiger storage engine.
1. The replica set or sharded cluster must use replica set protocol version 1.

Go to the [start free](https://www.mongodb.com/cloud/atlas/register) page and register for your free account.

# Running
You can run this console app and the use the [SimpleCrudExample1](https://github.com/madcodemonkey/MongoDB/tree/main/SimpleCrudExample1) project
in my Mongo repository to update the the data.  Just remember to update the connection string to point to MongoDB Atlas since it was first
designed to be used with a local Docker container.

# Compass Tool
If you want to look into the MongoDB running in the docker container, you can [download MongoDB's free tool called Compass](https://www.mongodb.com/try/download/compass).  

You can get the connection string from the Atlas portal. It will look something like this
```
"mongodb+srv://[userName]:[Password]@[yourclustername].[alphaNumberic].mongodb.net/"
```

# Documenation
- Change Streams: https://www.mongodb.com/basics/change-streams


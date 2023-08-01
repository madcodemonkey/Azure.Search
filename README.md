# Azure Search

## Projects List
- Create and Populate (CreateAndPopulateApp) - This is a console application designed to show how to create and populate Azure Cognitive Search indexes in code.  It has a menu system that allows you to try different options. 
   - Technology: Console Application
      - Note 1: Name = CreateAndPopulateApp.sln  
      - Note 2: Show how to programmatically create an index, data source, indexer
      - Note 3: Manually upload data using the NuGet package.
- CustomBlobIndexer - This is an example of a custom indexer that will monitor a Storage Container and create index documents
   - Technology: Azure Function (isolated)
      - Note 1: Name = CustomBlobIndexer.sln  
      - Note 2: Monitors changes using a BlobTrigger.  Note this is great for demos, but you should use Event Grid in production for monitoring changes!
      - Note 3: It has endpoints for creating the index 
      - Note 4: It has endpoints for querying the index
      - Note 5: It can call an Open AI endpoint as well if configured properly.
- CustomMongoDbIndexer - This is an example of a custom indexer that will monitor a MongoDB's change stream
   - Technology 1: Azure Function (isolated) 
      - Note 1: Name = IndexHelperApp.sln
      - Note 2: Used for creating the index and querying the Cognitive Search index as well as creating, updating and deleting MongoDB documents.
   - Technology 2: Worker Service 
      - Note 1: Name = WorkerServiceMongoIndexerApp.sln
      - Note 2: Used for long term monitoring of MongoDB change stream.  It will create, update and delete Cognitive Search index documents based on MongoDB changes.
- Web - This is a standalone backend server that demonstrates two different styles of for querying a Coginitive Search index. 
   - Technology: Web API
      - Note 1: Name = SearchWeb.sln
      - Note 2: It uses an out-of-the-box SQL IndexER to create index items.
      - Note 3: It uses EF migrations to create and seed the database its database  
Notes
- See the readme.md file in each project for setup instructions.


## Create a **Basic** SKU Azure Search 
<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3a%2f%2fraw.githubusercontent.com%2fmadcodemonkey%2fAzure.Search%2fDotNet6%2fARM-Files%2fAzureSearchBasicSku.json" target="_blank">
       <img src="https://aka.ms/deploytoazurebutton"/>
</a>
  
## Branching scheme
- DotNet6: .NET Core 6.0 example
- [Future] I will create a branch for each version of .NET 

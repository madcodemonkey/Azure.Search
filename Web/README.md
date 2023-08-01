# SQL Server out-of-the-box index and indexer example
---
# Summary
Currently on the backend web site is built for this project.  In the future, I need to build a client front end to access it.  
Meanwhile, you can use the postman collection in the web project to hit the backend endpoints.

There are is both a "well defined endpoint" for querying the hotel information and a "generic endpoint" that could be used to query any index.
This example aims to show two different approaches to querying data, one that is super specific and one that is more generic.

# Setup
You'll need to do the following:
1. In the Azure Portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
1. In the Azure Portal, create a SQL Server and then a SQL Database.  
   - Note 1: On the SQL SERVER resource under networkings, don't forget to check the checkbox labeled "Allow Azure services and resources to access this server" under "Exceptions"
   - Note 2: On the SQL SERVER resource under networkings, if you want to access the database from your computer using the SSMS application, don't forget to give access to "Selected Networks" under "Public network access" and add your IP address in the "Firewall rules" list.
1. Update the appsettingss.json entries with information from the Azure Portal
   - SearchService:SearchApiKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - SearchService:SearchEndPoint - This is what you complete endpoint url from the portal (e.g., https://[mycogsearchname].search.windows.net).
   - AcmeDatabaseOptions:ConnectionString - A connection string for your SQL server database (theses are listed under each database that belongs to the server you created).

# Running
1. Run the web site, Search.Web, project. By default, EF migrations will run and create the database and seed it with data.
1. Use postman collection to create the synonymn list, index, associate the synonym list to certain fields, create the data source, create the indexer and then run the indexer.  
   - Note: You'll find these under "Well Defined Endpoint/Hotel Creation" and each call is number from 01 - 06.  Just run them in order.
1. After the indexer finishes it's job, which should be fairly quick, you can use the queries defined in the postman collection under "Well Defined Endpoint/Hotel Queries" to query for data in the Azure Cognitive Search index.
   - Note: When creating the indexER if you get a failure due to IP address not being allowed, you forgot to update the network settings above (specially the "Allow Azure services and resources to access this server" setting)
1. There are also a queries under the "Generic Endpoint/Hotel Queries" folder in the postman collection.  Theses also allow you to query for data in the Cognitive Search index, but are meant to show off a more generic approach to querying indexes where this endpoint can be used to query any index that exists in your  Cognitive Search resource. 


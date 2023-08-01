# Create and popluate application
---
# Summary
This application is designed to show how to create and populate an index via code.  It can create an out-of-the-box SQL Server indexER.

# Setup
You'll need to do the following:
1. In the Azure Portal, create instance of an Azure Cognitive Search resource (Create a basic or S1 sku.  Remember that S1 sku will deplete an MSDN subscription before the end of the month).
1. In the Azure Portal, create a SQL Server and then a SQL Database.  
   - Note 1: On the SQL SERVER resource under networkings, don't forget to check the checkbox labeled "Allow Azure services and resources to access this server" under "Exceptions"
   - Note 2: On the SQL SERVER resource under networkings, if you want to access the database from your computer using the SSMS application, don't forget to give access to "Selected Networks" under "Public network access" and add your IP address in the "Firewall rules" list.
1. Update the appsettingss.json entries with information from the Azure Portal
   - SearchApiKey - Get the Primary or Secondary admin key from the Azure Portal.  See Setting section and the "Keys" item.
   - SearchEndPoint - This is what you complete endpoint url from the portal (e.g., https://[mycogsearchname].search.windows.net).
   - DatabaseConnectionString - A connection string for your SQL server database (theses are listed under each database that belongs to the server you created).

# Running
1. Run the CreateAndPopulateApp project
1. Used the console's menu system to do the following
    1. Create an index
	1. Create synonymns
	1. Create documents in the index.

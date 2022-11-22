using ConsoleMenuHelper;
using CreateAndPopulateApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Search.CogServices;
using Search.Services;

try
{
    var menu = new ConsoleMenu();

    menu.AddDependencies(AddMyDependencies);
    menu.AddMenuItemViaReflection(Assembly.GetExecutingAssembly());

    await menu.DisplayMenuAsync("Main", "Main");

    Console.WriteLine("Done!");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}
 

static void AddMyDependencies(IServiceCollection serviceCollection)
{

    // IConfiguration requires: Microsoft.Extensions.Configuration NuGet package
    // AddJsonFile requires:    Microsoft.Extensions.Configuration.Json NuGet package
    // https://stackoverflow.com/a/46437144/97803
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets(typeof(CreateHotelsIndexMenuItem).Assembly);

    IConfiguration config = builder.Build();

    serviceCollection.AddSingleton<IConfiguration>(config);

    var searchServiceSettings = GetSearchSettings(config);
    serviceCollection.AddCogServices(searchServiceSettings);
    serviceCollection.AddSearchServices(searchServiceSettings);
}


static SearchServiceSettings GetSearchSettings(IConfiguration config)
{

    return new SearchServiceSettings
    {
        SearchApiKey = config["SearchApiKey"],
        SearchEndPoint = config["SearchEndPoint"],

        // Database
        DatabaseConnectionString= config["DatabaseConnectionString"],

        // Azure SQL Settings
        HotelSuggestorName = config["HotelSuggestorName"],
        HotelTableName = config["HotelTableName"],
        HotelDataSourceName= config["HotelDataSourceName"],
        HotelIndexerName= config["HotelIndexerName"],
        HotelSynonymMapName= config["HotelSynonymMapName"],
        HotelIndexName= config["HotelIndexName"],
    };

}
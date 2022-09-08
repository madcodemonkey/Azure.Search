﻿using ConsoleMenuHelper;
using CreateAndPopulateApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
    serviceCollection.AddSearchServices(GetSearchSettings(config));
}

static SearchServiceSettings GetSearchSettings(IConfiguration config )
{
    return new SearchServiceSettings
    {
        SearchAdminApiKey = config["SearchServiceAdminApiKey"],
        SearchEndPoint = config["SearchServiceEndPoint"],
        SearchQueryApiKey = config["SearchServiceQueryApiKey"],

        // Azure SQL Settings
        SearchAzureSqlConnectionString= config["SearchServiceAzureSqlConnectionString"],
        SearchAzureSqlTableName = config["SearchServiceAzureSqlTableName"],
        SearchAzureSqlDataSourceName= config["SearchServiceAzureSqlDataSourceName"],
        SearchAzureSqlIndexerName= config["SearchServiceAzureSqlIndexerName"],
    };
}
﻿using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 2)]
public class AzureSqlDataSourceCreateMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly AppDatabaseOptions _databaseOptions;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchDataSourceService _dataSourceService;

    /// <summary>Constructor</summary>
    public AzureSqlDataSourceCreateMenuItem(SearchServiceSettings settings, AppDatabaseOptions databaseOptions,
        IPromptHelper promptHelper, IAcmeSearchDataSourceService dataSourceService)
    {
        _settings = settings;
        _databaseOptions = databaseOptions;
        _promptHelper = promptHelper;
        _dataSourceService = dataSourceService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {

        string dataSourceName = _promptHelper.GetText($"Name of the HOTEL Data Source (Default: {_settings.Hotel.DataSourceName})?", true, true);
        if (string.IsNullOrWhiteSpace(dataSourceName))
            dataSourceName = _settings.Hotel.DataSourceName;

        if (dataSourceName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        string tableName = _promptHelper.GetText($"Name of the Index (Default: {_settings.Hotel.TableName})?", true, true);
        if (string.IsNullOrWhiteSpace(tableName))
            tableName = _settings.Hotel.TableName;

        if (tableName == "exit")
            return new ConsoleMenuItemResponse(false, false);

        Console.WriteLine("Creating the data source that is needed for the indexer...");
        await _dataSourceService.CreateForAzureSqlAsync(dataSourceName, tableName, _databaseOptions.ConnectionString);

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Data Source Create  (needed before Indexer is created)";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
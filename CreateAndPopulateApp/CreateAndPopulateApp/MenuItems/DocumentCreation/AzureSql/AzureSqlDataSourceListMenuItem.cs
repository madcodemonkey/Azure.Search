﻿using ConsoleMenuHelper;
using Search.CogServices;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 1)]
public class AzureSqlDataSourceListMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchDataSourceService _dataSourceService;

    /// <summary>Constructor</summary>
    public AzureSqlDataSourceListMenuItem(IAcmeSearchDataSourceService dataSourceService)
    {
        _dataSourceService = dataSourceService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        List<string> dataSourceNameList = await _dataSourceService.GetListAsync();

        if (dataSourceNameList.Count == 0)
            Console.WriteLine("Nothing found.");

        foreach (var dataSourceName in dataSourceNameList)
        {
            Console.WriteLine(dataSourceName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Data Source List (show all of them)";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
﻿using ConsoleMenuHelper;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 4)]
public class AzureSqlIndexerListMenuItem : IConsoleMenuItem
{
    private readonly ISearchIndexerService _indexerService;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerListMenuItem(ISearchIndexerService indexerService)
    {
        _indexerService = indexerService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        List<string> indexerList = await _indexerService.GetIndexerListAsync();
        
        if (indexerList.Count == 0)
            Console.WriteLine("Nothing found.");

        foreach (var indexerName in indexerList)
        {
            Console.WriteLine(indexerName);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Indexer List (show all of them)";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
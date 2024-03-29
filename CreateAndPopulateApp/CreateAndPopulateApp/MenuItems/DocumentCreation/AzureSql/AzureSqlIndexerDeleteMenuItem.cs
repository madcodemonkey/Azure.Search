﻿using ConsoleMenuHelper;
using Search.CogServices;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("AzureSql", 5)]
public class AzureSqlIndexerDeleteMenuItem : IConsoleMenuItem
{
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;
    private readonly IAcmeSearchIndexerService _indexerService;

    /// <summary>Constructor</summary>
    public AzureSqlIndexerDeleteMenuItem(SearchServiceSettings settings, IPromptHelper promptHelper, IAcmeSearchIndexerService indexerService)
    {
        _settings = settings;
        _promptHelper = promptHelper;
        _indexerService = indexerService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexerName = _promptHelper.GetText($"Name of the Indexer to delete (Default: {_settings.Hotel.IndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexerName))
            indexerName = _settings.Hotel.IndexerName;

        if (_promptHelper.GetYorN("Are you sure you want to delete the indexer", true))
        {
            var result = await _indexerService.DeleteAsync(indexerName);
            if (result)
                Console.WriteLine($"Deleted {indexerName}");
            else Console.WriteLine("NOTHING deleted");
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Indexer delete";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
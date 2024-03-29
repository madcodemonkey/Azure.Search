﻿using ConsoleMenuHelper;
using Search.CogServices;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms")]
public class SynonymListAllMapsMenuItem : IConsoleMenuItem
{
    private readonly IAcmeSearchSynonymService _synonymService;

    /// <summary>Constructor</summary>
    public SynonymListAllMapsMenuItem(IAcmeSearchSynonymService synonymService)
    {
        _synonymService = synonymService;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        var response = await _synonymService.GetSynonymMapNamesAsync();
        foreach (string item in response)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "List all Synonyms Maps";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
using Azure;
using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 5)]
public class Query5HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly ISearchIndexService _indexService;
    private readonly IPromptHelper _promptHelper;
    private readonly string _defaultIndexName;

    public Query5HotelIndexMenuItem(ISearchIndexService indexService, IConfiguration configuration, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _promptHelper = promptHelper;
        _defaultIndexName = configuration["SearchServiceIndexName"];
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to search (Default: {_defaultIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _defaultIndexName;

        string keyName = _promptHelper.GetText("The document hotel id (Query first and get this. Leave blank to exit)?", true, true);

        if (string.IsNullOrWhiteSpace(keyName))
        {
            Console.WriteLine("You must specify a GUID that represents a hotel id.");
            return new ConsoleMenuItemResponse(false, false);
        }

        Console.WriteLine("Query #5: Look up a specific document...\n");

        Response<Hotel>? lookupResponse = await _indexService.GetDocumentAsync<Hotel>(indexName, keyName);

        if (lookupResponse != null)
        {
            Console.WriteLine(lookupResponse.Value);
        }
        else
        {
            Console.WriteLine("Not found");
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Query #5: Look up a specific document";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
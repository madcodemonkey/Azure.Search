using Azure;
using ConsoleMenuHelper;
using Search.Model;
using Search.Services;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Index", 5)]
public class Query5HotelIndexMenuItem : QueryHotelIndexMenuItemBase, IConsoleMenuItem
{
    private readonly IAcmeSearchIndexService _indexService;
    private readonly SearchServiceSettings _settings;
    private readonly IPromptHelper _promptHelper;

    public Query5HotelIndexMenuItem(IAcmeSearchIndexService indexService, SearchServiceSettings settings, IPromptHelper promptHelper)
    {
        _indexService = indexService;
        _settings = settings;
        _promptHelper = promptHelper;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        string indexName = _promptHelper.GetText($"Name of the index to search (Default: {_settings.HotelIndexName})?", true, true);
        if (string.IsNullOrWhiteSpace(indexName))
            indexName = _settings.HotelIndexName;

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
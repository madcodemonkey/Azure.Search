using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 2)]
public class SynonymMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    /// <summary>Constructor</summary>
    public SynonymMenuItem(IConsoleMenuController menuController)
    {
        _menuController = menuController;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        await _menuController.DisplayMenuAsync("Synonyms", "Synonyms", BreadCrumbType.Concatenate);
        return new ConsoleMenuItemResponse(false, true);
    }

    public string ItemText => "Synonyms";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
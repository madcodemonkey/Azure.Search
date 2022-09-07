using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main")]
public class SynonymsMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    public SynonymsMenuItem(IConsoleMenuController menuController)
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
    public string AttributeData { get; set; }
}
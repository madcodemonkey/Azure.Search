using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 3)]
public class DocumentCreationMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    /// <summary>Constructor</summary>
    public DocumentCreationMenuItem(IConsoleMenuController menuController)
    {
        _menuController = menuController;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        await _menuController.DisplayMenuAsync("DocumentCreation", "Document Creation", BreadCrumbType.Concatenate);
        return new ConsoleMenuItemResponse(false, true);
    }

    public string ItemText => "Document creation";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
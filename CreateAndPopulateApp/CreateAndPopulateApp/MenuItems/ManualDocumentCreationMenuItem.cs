using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 3)]
public class ManualDocumentCreationMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    /// <summary>Constructor</summary>
    public ManualDocumentCreationMenuItem(IConsoleMenuController menuController)
    {
        _menuController = menuController;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        await _menuController.DisplayMenuAsync("Manual", "Manual", BreadCrumbType.Concatenate);
        return new ConsoleMenuItemResponse(false, true);
    }

    public string ItemText => "Manual document creation";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
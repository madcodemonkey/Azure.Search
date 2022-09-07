using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 1)]
public class IndexMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    /// <summary>Constructor</summary>
    public IndexMenuItem(IConsoleMenuController menuController)
    {
        _menuController = menuController;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        await _menuController.DisplayMenuAsync("Index", "Indexes", BreadCrumbType.Concatenate);
        return new ConsoleMenuItemResponse(false, true);
    }

    public string ItemText => "Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
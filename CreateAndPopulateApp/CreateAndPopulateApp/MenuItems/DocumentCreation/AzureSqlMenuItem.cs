using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("DocumentCreation", 2)]
public class AzureSqlMenuItem : IConsoleMenuItem
{
    private readonly IConsoleMenuController _menuController;

    /// <summary>Constructor</summary>
    public AzureSqlMenuItem(IConsoleMenuController menuController)
    {
        _menuController = menuController;
    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        await _menuController.DisplayMenuAsync("AzureSql", "Azure Sql", BreadCrumbType.Concatenate);
        return new ConsoleMenuItemResponse(false, true);

    }

    public string ItemText => "Azure SQL";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;


}
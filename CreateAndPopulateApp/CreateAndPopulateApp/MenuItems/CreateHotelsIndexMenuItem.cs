using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 1)]
public class CreateHotelsIndexMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;

    public CreateHotelsIndexMenuItem(IConfiguration configuration, IPromptHelper promptHelper)
    {

    }

    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        int? numOfEvents  = _promptHelper.GetNumber("How many events would you like to send (0-10)?", 0, 10, "", 0);

        if (numOfEvents == null || numOfEvents.Value == 0)
        {
            Console.WriteLine("Nothing to send!");
            return new ConsoleMenuItemResponse(false, false);
        }

        Console.WriteLine("-------------------------------");

        return new ConsoleMenuItemResponse(false, false);
    }

    public string ItemText => "Create Hotels Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; }
}
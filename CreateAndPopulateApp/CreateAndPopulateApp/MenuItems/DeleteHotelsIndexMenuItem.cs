using ConsoleMenuHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Main", 2)]
public class DeleteHotelsIndexMenuItem : IConsoleMenuItem
{
    private readonly IPromptHelper _promptHelper;

    public DeleteHotelsIndexMenuItem(IConfiguration configuration, IPromptHelper promptHelper)
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

    public string ItemText => "Delete Hotels Index";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; }
}
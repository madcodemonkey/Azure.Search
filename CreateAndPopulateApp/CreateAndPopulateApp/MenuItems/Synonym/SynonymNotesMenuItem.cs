using ConsoleMenuHelper;

namespace CreateAndPopulateApp;

[ConsoleMenuItem("Synonyms", 6)]
public class SynonymNotesMenuItem : IConsoleMenuItem
{
    public async Task<ConsoleMenuItemResponse> WorkAsync()
    {
        var noteList = new List<string>
        {
            "You can see the synonym assignments for each field in the portal if you click on the index and then use the 'Edit JSON' button.  Examine the synonymMaps array for each field (not all fields were associated to a synonym map).",
            "A field can be associated to more than one synonym map.",
            "Synonym maps can be used by more than one index, which is why we have to associate our fields to them.",
        };

        foreach (string note in noteList)
        {
            Console.WriteLine($"-- {note}");
        }

        Console.WriteLine("-------------------------------");

        return await Task.FromResult(new ConsoleMenuItemResponse(false, false));
    }

    public string ItemText => "Notes";

    /// <summary>Optional data from the attribute.</summary>
    public string AttributeData { get; set; } = string.Empty;
}
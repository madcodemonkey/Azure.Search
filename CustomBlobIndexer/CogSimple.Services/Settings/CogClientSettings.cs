namespace CogSimple.Services;

public class CogClientSettings
{
    public const string SectionName = "CognitiveSearch";
    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string Key { get; set; } = string.Empty;
}
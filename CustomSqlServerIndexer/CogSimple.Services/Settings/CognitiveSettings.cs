namespace CogSimple.Services;

public class CognitiveSettings
{
    public const string SectionName = "Cognitive";

    /// <summary>
    /// The URI of the Azure Cognitive Search instance.
    /// You can find this on the overview tab of the resource.
    /// </summary>
    public string SearchEndpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string SearchKey { get; set; } = string.Empty;
    
    /// <summary>
    /// The URI of the Cognitive Services resource. These are used by skill sets to perform enrichment actions.
    /// </summary>
    public string ServicesEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// The key of the Cognitive Services resource.
    /// </summary>
    public string ServicesKey { get; set; } = string.Empty;
}
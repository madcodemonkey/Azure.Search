namespace OutOfTheBoxBlobIndexer.Services;

public class CogClientSettings
{
    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string CognitiveSearchEndpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string CognitiveSearchKey { get; set; } = string.Empty;
}
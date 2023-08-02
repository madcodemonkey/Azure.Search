namespace CustomBlobIndexer.Services;
 
public class ServiceSettings
{
     
    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; } = string.Empty;

    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string CognitiveSearchKey { get; set; } = string.Empty;

    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string CognitiveSearchName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;
  
    /// <summary>
    /// This is the endpoint of the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string CognitiveServiceEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// This is the key needed to access the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string CognitiveServiceKey { get; set; } = string.Empty;
 
}
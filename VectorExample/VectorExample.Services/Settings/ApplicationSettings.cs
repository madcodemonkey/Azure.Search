using Microsoft.Extensions.Options;

namespace VectorExample.Services;

public class ApplicationSettings 
{
    public const string SectionName = "AppSetting";
    
    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; } = string.Empty;

    
    /// <summary>
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;

    /// <summary>
    /// The vector configuration name
    /// </summary>
    public string VectorSearchConfigurationName { get; set; } = string.Empty;
}
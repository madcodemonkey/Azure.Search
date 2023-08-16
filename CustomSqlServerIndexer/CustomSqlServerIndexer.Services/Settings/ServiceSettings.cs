namespace CustomSqlServerIndexer.Services;
 
public class ServiceSettings
{
    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; } = string.Empty;
 
    /// <summary>
    /// When saving data to the Cognitive Search index, this is the maximum batch size.
    /// </summary>
    public int CognitiveSearchMaxUpsertBatchSize { get; set; }

    /// <summary>
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;
}
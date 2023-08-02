namespace IndexHelper.Models;

public class IndexAppSettings
{
    public string CognitiveSearchIndexName { get; set; } = string.Empty;
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;
    public string MongoDatabaseName { get; set; } = string.Empty;
    public string MongoPeopleCollection { get; set; } = string.Empty;
}
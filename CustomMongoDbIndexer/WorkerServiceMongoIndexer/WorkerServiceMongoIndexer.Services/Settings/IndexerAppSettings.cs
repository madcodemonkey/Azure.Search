namespace WorkerServiceMongoIndexer.Services;

public class IndexerAppSettings
{
    public string CognitiveSearchIndexName { get; set; } = string.Empty;

    public string MongoDatabaseName { get; set; } = string.Empty;
}
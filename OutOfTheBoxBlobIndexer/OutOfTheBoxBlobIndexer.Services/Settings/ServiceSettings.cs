﻿namespace OutOfTheBoxBlobIndexer.Services;
 
public class ServiceSettings
{

    /// <summary>
    /// The name of the data source 
    /// </summary>
    public string CognitiveSearchDataSourceName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string CognitiveSearchEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// The IndexER name
    /// </summary>
    public string CognitiveSearchIndexerName { get; set; } = String.Empty;

    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; } = string.Empty;
    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string CognitiveSearchKey { get; set; } = string.Empty;
    /// <summary>
    /// When saving data to the Cognitive Search index, this is the maximum batch size.
    /// </summary>
    public int CognitiveSearchMaxUpsertBatchSize { get; set; }

    /// <summary>
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;

    /// <summary>
    /// Within the Azure Storage resource, this is the name of the container that has files in it that you want in the index.
    /// </summary>
    public string CognitiveSearchStorageContainerName { get; set; }

    /// <summary>
    /// This is a connection string to attach to the Azure Storage resource
    /// </summary>
    public string CognitiveSearchStorageConnectionString { get; set; }
}
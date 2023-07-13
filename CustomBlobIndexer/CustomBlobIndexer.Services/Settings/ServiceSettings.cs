namespace CustomBlobIndexer.Services;

public class ServiceSettings
{
    /// <summary>
    /// An Access key to gain access to the storage account.  This could be key1 or key2 from the Access Keys section in the storage account 
    /// </summary>
    public string BlobAccessKey { get; set; }

    /// <summary>
    /// The name of the storage account.  This is the name of the storage resource in the Azure Portal.
    /// </summary>
    public string BlobAccountName { get; set; }

    /// <summary>
    /// The name of the container within the storage account (<see cref="BlobAccountName"/>) that contains the files we are watching.
    /// </summary>
    public string BlobContainerName { get; set; }

    /// <summary>
    /// The name of the index we are creating documents inside.
    /// </summary>
    public string CognitiveSearchIndexName { get; set; }

    /// <summary>
    /// The key that gets us access to the cognitive search instance.
    /// </summary>
    public string CognitiveSearchKey { get; set; }

    /// <summary>
    /// The name of the overall Azure Search instance.
    /// </summary>
    public string CognitiveSearchName { get; set; }

    /// <summary>
    /// This is the endpoint of the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string CognitiveServiceEndpoint { get; set; }

    /// <summary>
    /// This is the key needed to access the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string CognitiveServiceKey { get; set; }
}
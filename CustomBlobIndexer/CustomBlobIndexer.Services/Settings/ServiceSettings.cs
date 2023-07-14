namespace CustomBlobIndexer.Services;

public class ServiceSettings
{
    /// <summary>
    /// An Access key to gain access to the storage account.  This could be key1 or key2 from the Access Keys section in the storage account 
    /// </summary>
    public string BlobAccessKey { get; set; } = string.Empty;

    /// <summary>
    /// The name of the storage account.  This is the name of the storage resource in the Azure Portal.
    /// </summary>
    public string BlobAccountName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the container within the storage account (<see cref="BlobAccountName"/>) that contains the files we are watching.
    /// </summary>
    public string BlobContainerName { get; set; } = string.Empty;

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
    /// Determines if we should detect entities from the content and attach that result to the document.
    /// </summary>
    public bool CognitiveSearchSkillDetectEntities { get; set; }

    /// <summary>
    /// Determines if we should detect key phrases from the content and attach that result to the document.
    /// </summary>
    public bool CognitiveSearchSkillDetectKeyPhrases { get; set; }

    /// <summary>
    /// Determines if we should detect languages from the content and attach that result to the document.
    /// </summary>
    public bool CognitiveSearchSkillDetectLanguage { get; set; }

    /// <summary>
    /// Determines if we should detect sentiment from the content and attach that result to the document.
    /// </summary>
    public bool CognitiveSearchSkillDetectSentiment { get; set; }

    /// <summary>
    /// Determines if we should redact certain pieces of data from the content and attach that result to the document.
    /// </summary>
    public bool CognitiveSearchSkillRedactText { get; set; }

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
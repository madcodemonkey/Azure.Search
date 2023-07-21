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
    /// If false, it indicates that we don't want to break incoming document apart into multiple documents in the index.  If true, you are
    /// most likely doing this for OpenAI, so we will break the documents down so that no document has more
    /// than <see cref="ChunkMaximumNumberOfCharacters"/> characters in it to avoid overwhelming the Open API with too many tokens.
    /// </summary>
    public bool ChunkIntoMultipleDocuments { get; set; }

    /// <summary>
    /// The maximum number of characters in a document when <see cref="ChunkIntoMultipleDocuments"/>
    /// is true. It's not used when <see cref="ChunkIntoMultipleDocuments"/> is false.
    /// </summary>
    public int ChunkMaximumNumberOfCharacters { get; set; } = 1000;

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
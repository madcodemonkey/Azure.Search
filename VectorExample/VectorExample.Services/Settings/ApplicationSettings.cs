using Microsoft.Extensions.Options;

namespace VectorExample.Services;

public class ApplicationSettings 
{
    public const string SectionName = "AppSetting";

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
    /// The name of the semantic configuration to setup within the index.
    /// </summary>
    public string CognitiveSearchSemanticConfigurationName { get; set; } = string.Empty;

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

    public bool CognitiveSearchSkillSummarizeText { get; set; }
}
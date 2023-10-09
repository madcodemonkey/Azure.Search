namespace VectorExample.Services;

public class CognitiveServiceSettings
{
    public const string SectionName = "CognitiveService";

    /// <summary>
    /// This is the endpoint of the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// This is the key needed to access the Cognitive Service endpoint (NOT the Cognitive Search endpoint!).
    /// This service is used for OCR and languages services that are needed by skillsets.
    /// </summary>
    public string Key { get; set; } = string.Empty;
}
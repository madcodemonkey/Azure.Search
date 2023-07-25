namespace CustomBlobIndexer.Models;

public class OpenAIRequest
{
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// The name of the field that should be used by Cognitive Search (case matters) when searching for data. 
    /// </summary>
    public string SearchFieldName { get; set; } = string.Empty;

    /// <summary>
    /// A statement about the current situation.
    /// </summary>
    public string SituationStatement { get; set; }
}
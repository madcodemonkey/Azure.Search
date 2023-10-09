namespace CustomBlobIndexer.Services;

public class OpenAiSettings
{
    public const string SectionName = "OpenAI";

    /// <summary>
    /// Open AI - this is the name of your deployment model that you are using (see the Azure AI Studio for this information).
    /// </summary>
    public string DeploymentOrModelName { get; set; } = string.Empty;

    /// <summary>
    /// Open AI - this is the endpoint URI for the open ai resource (see the Azure Portal -- Keys and Endpoint under Resource Management -- for this information).
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Open AI - this is either key 1 or key 2 for the open ai resource (see the Azure Portal -- Keys and Endpoint under Resource Management -- for this information).
    /// </summary>
    public string Key { get; set; } = string.Empty;
}
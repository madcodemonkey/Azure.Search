using Azure.Search.Documents.Indexes;

namespace OutOfTheBoxBlobIndexer.Models;

public class SearchIndexDocument
{
    /// <summary>
    /// The id field (it's current a base64 encoded string to the entire path of the file.
    /// </summary>
    /// <remarks>Maps to: metadata_storage_path</remarks>
    [SearchableField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// This is the text that was inside of the file.
    /// </summary>
    /// <remarks>Maps to: content</remarks>
    [SearchableField(IsFilterable = true, IsSortable = true, AnalyzerName = "standard.lucene")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The file content type (e.g., "application/pdf").  
    /// </summary>
    /// <remarks>Maps to:  metadata_content_type</remarks>
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// The file extension (e.g., ".pdf").  Yes, it includes the leading period.
    /// </summary>
    /// <remarks>Maps to:  metadata_storage_file_extension</remarks>
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// The file name (e.g., ""Example1-History.pdf").  This does not include the path inside the container.
    /// </summary>
    /// <remarks>Maps to:  metadata_storage_name</remarks>
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public string FileName { get; set; } = string.Empty;

}
 
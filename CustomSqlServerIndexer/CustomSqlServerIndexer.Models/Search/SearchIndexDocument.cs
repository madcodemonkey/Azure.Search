using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace CustomSqlServerIndexer.Models;

public class SearchIndexDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// If we are chunking documents (breaking them apart), this is the order they can be
    /// recombined; thus, you could use the <see cref="SourcePath"/> to retrieve them and then sort
    /// them by <see cref="ChunkOrderNumber"/> and reassemble all the text.
    /// </summary>
    [SimpleField(IsFilterable = true, IsSortable = true)]
    public int ChunkOrderNumber { get; set; }

    [SearchableField(IsSortable = true)]
    public string Title { get; set; } = string.Empty;

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Content { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string SourcePath { get; set; } = string.Empty;

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public List<string> KeyPhrases { get; set; } = new List<string>();

    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public List<SearchEntity> Entities { get; set; } = new List<SearchEntity>();

    
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Summary { get; set; } = string.Empty;


    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string RedactedText { get; set; } = string.Empty;
}
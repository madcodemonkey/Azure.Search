using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace VectorExample.Models;

public class SearchIndexDocument
{
    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string Category { get; set; }

    [SearchableField(IsFilterable = true, AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Content { get; set; }

    [SearchableField]
    public IReadOnlyList<float> ContentVector { get; set; }

    [SimpleField(IsKey = true, IsFilterable = true, IsSortable = true)]
    public string Id { get; set; }

    [SearchableField(IsSortable = true, IsFilterable = true)]
    public string Title { get; set; }

    [SearchableField]
    public IReadOnlyList<float> TitleVector { get; set; }

    [SimpleField(IsFilterable = true)]
    public int VectorEmbeddingVersion { get; set; }
}
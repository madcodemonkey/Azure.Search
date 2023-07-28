using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace IndexHelper.Models;

public class SearchIndexDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; }

    /// <summary>
    /// If we are chunking documents (breaking them apart), this is the order they can be
    /// recombined; thus, you could use the <see cref="SourcePath"/> to retrieve them and then sort
    /// them by <see cref="ChunkOrderNumber"/> and reassemble all the text.
    /// </summary>
    [SimpleField(IsFilterable = true, IsSortable = true)]
    public int ChunkOrderNumber { get; set; }

    [SearchableField(IsSortable = true)]
    public string Title { get; set; }
   
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Content { get; set; }
    
    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string SourcePath { get; set; }
    
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public List<string> KeyPhrases { get; set; }
 
    
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Summary { get; set; }
  
    
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string RedactedText { get; set; }
}
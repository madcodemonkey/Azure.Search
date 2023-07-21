using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace CustomBlobIndexer.Models;

public class SearchIndexDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public string Id { get; set; }

    /// <summary>
    /// If we are chunking documents (breaking them apart), this id will be the same for ALL the chunks.
    /// </summary>
    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    public string ChunkId { get; set; }

    /// <summary>
    /// If we are chunking documents (breaking them apart), this is the order they can be
    /// recombined; thus, you could use the <see cref="ChunkId"/> to retrieve them and then sort
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
 
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public List<SearchEntity> Entities { get; set; }

    // [SearchableField(IsFilterable = true, IsFacetable = true)]
    // public List<SearchSentiment> Sentiments { get; set; }
   
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Summary { get; set; }
  
    [SearchableField(IsFilterable = true, IsFacetable = true)]
    public List<SearchLanguage> Languages { get; set; }
 
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string RedactedText { get; set; }
}
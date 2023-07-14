using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Newtonsoft.Json;

namespace CustomBlobIndexer.Models;

public class SearchIndexDocument
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    [JsonProperty("id")]
    public string Id { get; set; }

    [SearchableField(IsSortable = true)]
    public string Title { get; set; }
   
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string Content { get; set; }
    
    [SearchableField(IsFilterable = true, IsSortable = true, IsFacetable = true)]
    [JsonProperty("source")]
    public string Source { get; set; }
    
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
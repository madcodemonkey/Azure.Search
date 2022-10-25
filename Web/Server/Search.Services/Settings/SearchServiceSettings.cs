namespace Search.Services;

public class SearchServiceSettings
{
    public string SearchEndPoint { get; set; }
    public string SearchApiKey { get; set; }
    public SearchServiceIndexSettings Hotel { get; set; }
    public SearchServiceSynonymSettings Synonyms { get; set; }
}
using Search.CogServices;

namespace Search.Services;

public class SearchServiceSettings : AcmeSearchSettings
{
    public SearchServiceIndexSettings Hotel { get; set; }
    public SearchServiceSynonymSettings Synonyms { get; set; }
}
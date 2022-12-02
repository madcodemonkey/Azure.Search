namespace Search.CogServices;

/// <summary>Used by either the suggest or autocomplete services</summary>
public class AcmeSuggestorQuery 
{
    private int _numberOfSuggestionsToRetrieve = 5;
    
    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterField> Filters { get; set; } = new List<AcmeSearchFilterField>();

    /// <summary>The number of suggestions to retrieve. This must be a value between 1 and 100.</summary>
    public int NumberOfSuggestionsToRetrieve
    {
        get => _numberOfSuggestionsToRetrieve;
        set => _numberOfSuggestionsToRetrieve = value > 0 ? value : 5;
    }

    /// <summary> A value indicating whether to use fuzzy matching for the suggestion query. Default is false.
    /// When set to true, the query will find suggestions even if there's a substituted or missing character in
    /// the search text. While this provides a better experience in some scenarios, it comes at a performance cost
    /// as fuzzy suggestion searches are slower and consume more resources. </summary>
    public bool UseFuzzyMatching { get; set; } = false;
}
namespace Search.CogServices;

/// <summary>Used by either the suggest or autocomplete services</summary>
public class AcmeSuggestorQueryV2
{
    private int _numberOfSuggestionsToRetrieve = 5;

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterFieldV2>? Filters { get; set; }

    /// <summary>The number of suggestions to retrieve. This must be a value between 1 and 100.</summary>
    public int NumberOfSuggestionsToRetrieve
    {
        get => _numberOfSuggestionsToRetrieve;
        set => _numberOfSuggestionsToRetrieve = value > 0 ? value : 5;
    }


    /// <summary> A string tag that is appended to hit highlights. Must be set with highlightPreTag. Default is &amp;lt;/em&amp;gt;. </summary>
    public string? HighlightPostTag { get; set; }

    /// <summary> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. Default is &amp;lt;em&amp;gt;. </summary>
    public string? HighlightPreTag { get; set; }

    /// <summary>
    /// The names of the fields that we should retrieve in the document.  If left null, then only the id field will be retrieved.
    /// </summary>
    public IList<string>? DocumentFields { get; set; }

    /// <summary>
    /// The names of the fields that should be involved in the suggest query.
    /// </summary>
    public IList<string>? SelectFields { get; set; }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; } = string.Empty;

    /// <summary> A value indicating whether to use fuzzy matching for the suggestion query. Default is false.
    /// When set to true, the query will find suggestions even if there's a substituted or missing character in
    /// the search text. While this provides a better experience in some scenarios, it comes at a performance cost
    /// as fuzzy suggestion searches are slower and consume more resources. </summary>
    public bool UseFuzzyMatching { get; set; } = false;

    /// <summary>
    /// The names of the fields that should be used as facets.
    /// </summary>
    public IList<string>? SearchFields { get; set; }

    /// <summary>
    /// The name of the Azure Search Index
    /// </summary>
    public string IndexName { get; set; }


    /// <summary>
    /// The name of the Azure Search suggestor for this index (currently, despite the fact that is an array when creating it, you can only have one)
    /// </summary>
    public string SuggestorName { get; set; }
}
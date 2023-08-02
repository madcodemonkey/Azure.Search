using Azure.Search.Documents.Models;

namespace Search.CogServices;

/// <summary>Used by the autocomplete services</summary>
public class AcmeAutoCompleteQuery
{
    private int _numberOfSuggestionsToRetrieve = 5;

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterField>? Filters { get; set; }

    /// <summary> A string tag that is appended to hit highlights. Must be set with highlightPreTag. Default is &amp;lt;/em&amp;gt;. </summary>
    public string? HighlightPostTag { get; set; }

    /// <summary> A string tag that is prepended to hit highlights. Must be set with highlightPostTag. Default is &amp;lt;em&amp;gt;. </summary>
    public string? HighlightPreTag { get; set; }

    /// <summary>
    /// The name of the Azure Search Index
    /// </summary>
    public string IndexName { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the mode for Autocomplete. The default is
    /// <see cref="AutocompleteMode.OneTerm"/>. Use
    /// <see cref="AutocompleteMode.TwoTerms"/> to get shingles and
    /// <see cref="AutocompleteMode.OneTermWithContext"/> to use the
    /// current context while producing auto-completed terms.
    /// </summary>
    public AutocompleteMode? Mode { get; set; }

    /// <summary>The number of suggestions to retrieve. This must be a value between 1 and 100.</summary>
    public int NumberOfSuggestionsToRetrieve
    {
        get => _numberOfSuggestionsToRetrieve;
        set => _numberOfSuggestionsToRetrieve = value > 0 ? value : 5;
    }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// The names of the fields that should be used as facets.
    /// </summary>
    public IList<string>? SearchFields { get; set; }

    /// <summary>
    /// The name of the Azure Search suggestor for this index (currently, despite the fact that is an array when creating it, you can only have one)
    /// </summary>
    public string SuggestorName { get; set; } = string.Empty;

    /// <summary> A value indicating whether to use fuzzy matching for the suggestion query. Default is false.
    /// When set to true, the query will find suggestions even if there's a substituted or missing character in
    /// the search text. While this provides a better experience in some scenarios, it comes at a performance cost
    /// as fuzzy suggestion searches are slower and consume more resources. </summary>
    public bool? UseFuzzyMatching { get; set; }
}
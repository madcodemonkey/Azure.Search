using Search.CogServices;
using Search.CogServices.Extensions;

namespace Search.Web.Models;

/// <summary>Used by either the suggest or autocomplete services</summary>
public class AcmeSuggestQueryDto
{
    private int _numberOfSuggestionsToRetrieve = 5;

    /// <summary>
    /// Used to remap field on the document
    /// </summary>
    public IList<SearchDocumentFieldMap>? DocumentFieldMaps { get; set; }

    /// <summary>
    /// The names of the fields that we should retrieve in the document.  If left null, then only the id field will be retrieved.
    /// </summary>
    public IList<string>? DocumentFields { get; set; }

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterField>? Filters { get; set; }

    /// <summary>The number of suggestions to retrieve. This must be a value between 1 and 100.</summary>
    public int NumberOfSuggestionsToRetrieve
    {
        get => _numberOfSuggestionsToRetrieve;
        set => _numberOfSuggestionsToRetrieve = value > 0 ? value : 5;
    }

    /// <summary>A list of fields that you want to use to order the results.  You can have more than one order by
    /// and the order they appear in this list matters!!  If nothing is specified, we will sort by Score order
    /// descending order (highest to lowest score)</summary>
    public List<AcmeSearchOrderBy>? OrderByFields { get; set; }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// The names of the fields that should be used as facets.
    /// </summary>
    public IList<string>? SearchFields { get; set; }

    /// <summary> A value indicating whether to use fuzzy matching for the suggestion query. Default is false.
    /// When set to true, the query will find suggestions even if there's a substituted or missing character in
    /// the search text. While this provides a better experience in some scenarios, it comes at a performance cost
    /// as fuzzy suggestion searches are slower and consume more resources. </summary>
    public bool? UseFuzzyMatching { get; set; }
}
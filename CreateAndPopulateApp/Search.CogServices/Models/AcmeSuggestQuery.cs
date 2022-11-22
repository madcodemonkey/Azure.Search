namespace Search.CogServices;

public class AcmeSuggestQuery 
{
    private int _numberOfSuggestionsToRetrieve;

    /// <summary>Constructor</summary>
    public AcmeSuggestQuery()
    {
        Filters = new List<AcmeSearchFilterItem>();
    }

    /// <summary>The simple or Lucene style query</summary>
    public string Query { get; set; }

    /// <summary>Filters to narrow the search.  This help with response time a lot.</summary>
    /// <remarks>I'm not letting the user build them because filters are also part of security</remarks>
    public List<AcmeSearchFilterItem> Filters { get; set; }

    /// <summary>The number of suggestions to retrieve. This must be a value between 1 and 100.</summary>
    public int NumberOfSuggestionsToRetrieve
    {
        get => _numberOfSuggestionsToRetrieve;
        set => _numberOfSuggestionsToRetrieve = value > 0 ? value : 5;
    }
}
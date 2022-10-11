namespace Search.Model;

public class SearchSynonymCreationRequest
{
    /// <summary>Name of the new synonym list that can be used with any index's fields.</summary>
    public string Name { get; set; }

    /// <summary>A line return delimited list of synonyms.  See Microsoft's documentation here
    /// <see href="https://learn.microsoft.com/en-us/azure/search/search-synonyms"/> </summary>
    public string SynonymnList { get; set; }
}
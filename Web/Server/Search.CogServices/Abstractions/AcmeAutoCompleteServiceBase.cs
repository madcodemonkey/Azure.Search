using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public abstract class AcmeAutoCompleteServiceBase
{
    /// <summary>Constructor</summary>
    protected AcmeAutoCompleteServiceBase(IAcmeSearchIndexService searchIndexService, IAcmeFieldService fieldService)
    {
        SearchIndexService = searchIndexService;
        FieldService = fieldService;
    }


    protected IAcmeSearchIndexService SearchIndexService { get; }
    protected IAcmeFieldService FieldService { get; }
    protected abstract string IndexName { get; }
    protected abstract string SuggestorName { get; }

    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeSuggestorQuery request, List<string> rolesTheUserIsAssigned)
    {
        var options = CreateDefaultOptions(request, rolesTheUserIsAssigned);

        return await AutoCompleteAsync(request, options);
    }

    /// <summary>Autocomplete</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<Response<AutocompleteResults>> AutoCompleteAsync(AcmeSuggestorQuery request, AutocompleteOptions options)
    {
        Response<AutocompleteResults> autoCompleteResult = await SearchIndexService.AutocompleteAsync(IndexName, request.Query, SuggestorName, options);
        
        return autoCompleteResult;
    }


    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected virtual AutocompleteOptions CreateDefaultOptions(AcmeSuggestorQuery request, List<string> rolesTheUserIsAssigned)
    {
        var options = new AutocompleteOptions
        {
            Filter = FieldService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned),
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            Mode = AutocompleteMode.TwoTerms,
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = request.UseFuzzyMatching // the default is false for performance reasons.  My experience shows that it really does not work well with autocomplete, but works fine with Suggest.
        };

        AddFieldNamesToSearchFields(GetFieldNamesToSearch(), options);

        return options;
    }

    /// <summary>Adds a list of fields to the Select property.  This determines which of the document
    /// fields are returned along with the suggestion.</summary>
    /// <param name="fieldNames">The document field names to retrieve.</param>
    /// <param name="options">The options to add the field names to.</param>
    protected virtual void AddFieldNamesToSearchFields(List<string> fieldNames, AutocompleteOptions options)
    {
        options.SearchFields.Clear();

        foreach (string fieldName in fieldNames)
        {
            options.SearchFields.Add(fieldName);
        }
    }

    /// <summary>Gets a list of fields that we should be returned with the document found with the autocomplete.
    /// If left blank, it will return the key field. Warning! Field names must match exactly how they
    /// appear in the Azure Search Document!</summary>
    protected virtual List<string> GetFieldNamesToSearch()
    {
        return new List<string>();
    }
}
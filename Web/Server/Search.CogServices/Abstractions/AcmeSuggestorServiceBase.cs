using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public abstract class AcmeSuggestorServiceBase<TIndexClass> where TIndexClass : class
{
    /// <summary>Constructor</summary>
    protected AcmeSuggestorServiceBase(IAcmeSearchIndexService searchIndexService, IAcmeFieldService fieldService)
    {
        SearchIndexService = searchIndexService;
        FieldService = fieldService;
    }


    protected IAcmeSearchIndexService SearchIndexService { get; }
    protected IAcmeFieldService FieldService { get; }
    protected abstract string IndexName { get; }
    protected abstract string SuggestorName { get; }

    /// <summary>Suggest</summary>
    /// <param name="request">A request for a suggestion</param>
    /// <param name="rolesTheUserIsAssigned">Case sensitive list of roles that for search trimming.</param>
    /// <returns>List of suggestions</returns>
    public virtual async Task<SuggestResults<TIndexClass>> SuggestAsync(AcmeSuggestQuery request, List<string> rolesTheUserIsAssigned)
    {
        var options = CreateDefaultOptions(request, rolesTheUserIsAssigned);

        var suggestResult = await SearchIndexService.SuggestAsync<TIndexClass>(IndexName, request.Query, SuggestorName, options);

        return suggestResult;
    }
     
    /// <summary>Creates a set of default options you can then override if necessary.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    protected virtual SuggestOptions CreateDefaultOptions(AcmeSuggestQuery request, List<string> rolesTheUserIsAssigned)
    {
        var options = new SuggestOptions
        {
            Filter = FieldService.BuildODataFilter(request.Filters, rolesTheUserIsAssigned),
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            // MinimumCoverage = 33.3,
            OrderBy = { "search.score() desc" },
            //SearchFields = {  },
            // Select = { },
            Size = request.NumberOfSuggestionsToRetrieve,
            UseFuzzyMatching = false // false for performance reasons
        };

        AddFieldNamesToSelect(GetFieldNamesToSelect(), options);

        return options;
    }

    /// <summary>Adds a list of fields to the Select property.  This determines which of the document
    /// fields are returned along with the suggestion.</summary>
    /// <param name="fieldNames">The document field names to retrieve.</param>
    /// <param name="options">The options to add the field names to.</param>
    protected virtual void AddFieldNamesToSelect(List<string> fieldNames, SuggestOptions options)
    {
        options.Select.Clear();

        foreach (string fieldName in fieldNames)
        {
            options.Select.Add(fieldName);
        }
    }

    /// <summary>Gets a list of fields that we should be returned with the document found with the suggestion. If left blank, it will return the key field.
    /// Warning! Field names must match exactly how they appear in the Azure Search Document!</summary>
    protected virtual List<string> GetFieldNamesToSelect()
    {
        return new List<string>();
    }
}
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSearchHighlightService<TIndexClass> where TIndexClass : class
{
    /// <summary>Maps the highlight fields onto the document object.  This is handy if you don't really need the original document text
    /// and just want highlight text returned in the document object.</summary>
    /// <param name="docs">The docs that were found by the Azure Search method.</param>
    /// <param name="dividerBetweenHighlights">The text you would like between the highlights if more than one is found for a given field.
    /// In a field that contains a LOT of text, it's possible that you could get more than one highlight in different parts of the document.
    /// Sometimes it looks like it breaks it up by sentences and sometimes not.  This is what you want between the highlights.  It may not
    /// make any sense if the user sees them all smashed together so two breaks is the default.</param>
    /// <exception cref="ArgumentException">If you give us a property name that doesn't exist, you could get an exception.</exception>
    void MapHighlightsOnToDocumentSeparatorStyle(List<SearchResult<TIndexClass>> docs, string dividerBetweenHighlights = "<br/><br/>");

    /// <summary>Combines highlights, but limits the number of characters displayed.
    /// Each document has a Highlights list (e.g., azSearchResult.Docs[0].Highlights) which MAY have 1 or more sentences in it
    /// for a single field if matches were found.  This service will combine all the highlights for a single field into a
    /// StringBuilder and then overwrite the Document field.</summary>
    /// <param name="docs">The docs that were found by the Azure Search method.</param>
    /// <param name="maxNumberOfCharacters">The maximum number of characters to display.</param>
    void MapHighlightsOnToDocumentGoogleStyle(List<SearchResult<TIndexClass>> docs, int maxNumberOfCharacters);
}
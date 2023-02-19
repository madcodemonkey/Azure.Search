using Azure.Search.Documents.Models;
using System.Text;

namespace Search.CogServices;

// TODO: Move into a standalone highlight helper!
//public abstract class AcmeSearchHighlightServiceBase<TIndexClass> : IAcmeSearchHighlightService<TIndexClass>
//    where TIndexClass : class
//{
//    /// <summary>Constructor</summary>
//    protected AcmeSearchHighlightServiceBase(IAcmeFieldService fieldService)
//    {
//        FieldService = fieldService;
//    }

//    protected IAcmeFieldService FieldService { get; set; }

//    /// <summary>Combines highlights, but limits the number of characters displayed.
//    /// Each document has a Highlights list (e.g., azSearchResult.Docs[0].Highlights) which MAY have 1 or more sentences in it
//    /// for a single field if matches were found.  This service will combine all the highlights for a single field into a
//    /// StringBuilder and then overwrite the Document field.</summary>
//    /// <param name="docs">The docs that were found by the Azure Search method.</param>
//    /// <param name="maxNumberOfCharacters">The maximum number of characters to display.</param>
//    public virtual void MapHighlightsOnToDocumentGoogleStyle(List<SearchResult<TIndexClass>> docs, int maxNumberOfCharacters)
//    {
//        var sbResult = new StringBuilder();

//        foreach (SearchResult<TIndexClass> oneDocument in docs)
//        {
//            if (oneDocument.Highlights == null) continue;

//            foreach (var oneDocumentHighlight in oneDocument.Highlights)
//            {
//                var indexFieldName = FieldService.FindByIndexFieldName(oneDocumentHighlight.Key);
//                if (indexFieldName == null)
//                {
//                    throw new ArgumentException(
//                        $"{nameof(MapHighlightsOnToDocumentGoogleStyle)} failed.  The field service does not know about an " +
//                        $"Azure Index field named {oneDocumentHighlight.Key} so we were unable to map the field highlight over top the document field!  " +
//                        $"Please register the '{oneDocumentHighlight.Key}' field in the field service!");
//                }

//                var prop = typeof(TIndexClass).GetProperty(indexFieldName.PropertyFieldName);
//                if (prop == null)
//                {
//                    throw new ArgumentException($"{nameof(MapHighlightsOnToDocumentGoogleStyle)} failed.  " +
//                        $"Unable to find a property named {indexFieldName.PropertyFieldName} on the '{typeof(TIndexClass)}' class!");
//                }

//                sbResult.Clear();
//                bool addTripleDots = false;

//                // For each highlight
//                foreach (string sentence in oneDocumentHighlight.Value)
//                {
//                    // one sentence
//                    foreach (var word in sentence.Split(' '))
//                    {
//                        // Keep adding entire words till we gte a word that would exceed the max lenght.
//                        if (sbResult.Length + word.Length > maxNumberOfCharacters)
//                        {
//                            addTripleDots = true;
//                            break;
//                        }

//                        if (sbResult.Length > 0)
//                            sbResult.Append(" ");

//                        sbResult.Append(word);
//                    }

//                    // Make sure each sentences ends in a period.
//                    if (sbResult.Length > 0 && sbResult[^1] != '.')
//                    {
//                        sbResult.Append(".");
//                    }
//                }

//                // if we would have exceeded the max length, add the triple dots an exit
//                if (addTripleDots)
//                {
//                    sbResult.Append(" ...");
//                }

//                prop.SetValue(oneDocument.Document, sbResult.ToString());
//            }
//        }
//    }

//    /// <summary>Maps the highlight fields onto the document object.  This is handy if you don't really need the original document text
//    /// and just want highlight text returned in the document object.</summary>
//    /// <param name="docs">The docs that were found by the Azure Search method.</param>
//    /// <param name="dividerBetweenHighlights">The text you would like between the highlights if more than one is found for a given field.
//    /// In a field that contains a LOT of text, it's possible that you could get more than one highlight in different parts of the document.
//    /// Sometimes it looks like it breaks it up by sentences and sometimes not.  This is what you want between the highlights.  It may not
//    /// make any sense if the user sees them all smashed together so two breaks is the default.</param>
//    /// <exception cref="ArgumentException">If you give us a property name that doesn't exist, you could get an exception.</exception>
//    public virtual void MapHighlightsOnToDocumentSeparatorStyle(List<SearchResult<TIndexClass>> docs, string dividerBetweenHighlights = "<br/><br/>")
//    {
//        var sb = new StringBuilder();

//        foreach (SearchResult<TIndexClass> oneDoc in docs)
//        {
//            if (oneDoc.Highlights == null) continue;

//            foreach (var oneDocHighlight in oneDoc.Highlights)
//            {
//                if (oneDocHighlight.Value == null || oneDocHighlight.Value.Count == 0) continue;

//                var searchField = FieldService.FindByIndexFieldName(oneDocHighlight.Key);

//                if (searchField == null)
//                    throw new ArgumentException($"The field service could not find a property on the {typeof(TIndexClass)} named {oneDocHighlight.Key}!");

//                var prop = typeof(TIndexClass).GetProperty(searchField.PropertyFieldName);
//                if (prop == null) throw new ArgumentException($"There is no property on the {typeof(TIndexClass)} named {searchField.PropertyFieldName}!");

//                // You'll have more than one string if the text property being search is very large.
//                // The individual strings will correspond to sentences most of the time, but not always.
//                sb.Clear();
//                foreach (string oneHighlight in oneDocHighlight.Value)
//                {
//                    if (sb.Length > 0)
//                        sb.Append(dividerBetweenHighlights);
//                    sb.Append(oneHighlight);
//                }

//                prop.SetValue(oneDoc.Document, sb.ToString());
//            }
//        }
//    }
//}
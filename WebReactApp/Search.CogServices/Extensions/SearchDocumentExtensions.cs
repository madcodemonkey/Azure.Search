using Azure.Search.Documents.Models;

namespace Search.CogServices.Extensions;

public static class SearchDocumentExtensions
{
    /// <summary>
    /// Remaps source fields to new destinations.
    /// </summary>
    /// <param name="document">The document to change</param>
    /// <param name="fieldMaps">The fields to remap</param>
    public static void ReMapFields(this SearchDocument document, IList<SearchDocumentFieldMap>? fieldMaps)
    {
        if (fieldMaps == null) return;

        foreach (var fieldMap in fieldMaps)
        {
            // If the source doesn't exist, we can stop because there is nothing to copy
            if (document.ContainsKey(fieldMap.Source) == false)
                continue;

            // We do not want to overwrite existing data, so stop.
            if (document.ContainsKey(fieldMap.Destination))
                continue;

            // Copy the source value to the destination
            document.Add(fieldMap.Destination, document[fieldMap.Source]);
        }
    }

    /// <summary>
    /// Removes a field, if it exists, from the specified document.
    /// </summary>
    /// <param name="document">The document to change</param>
    /// <param name="fieldName">The field to remove.</param>
    public static void RemoveField(this SearchDocument document, string? fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName)) return;

        if (document.ContainsKey(fieldName) == false) return;

        document.Remove(fieldName);
    }
}
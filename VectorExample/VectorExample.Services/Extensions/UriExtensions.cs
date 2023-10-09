namespace CustomBlobIndexer.Services;

public static class UriExtensions
{
    /// <summary>
    /// Returns the portion of the path after the text specified.
    /// </summary>
    /// <param name="uri">The URI to examine</param>
    /// <param name="afterThisText">The text, which is not included, to look for.</param>
    /// <param name="trimOffLeadingSlash">Indicates if the resulting path has a leading slash that you want to remove it.</param>
    /// <returns>The path after the text.</returns>
    public static string GetPathAfterText(this Uri uri, string afterThisText, bool trimOffLeadingSlash = true)
    {
        string uriPath = uri.ToString();
        if (string.IsNullOrWhiteSpace(uriPath))
        {
            return string.Empty;
        }

        int index = uriPath.IndexOf(afterThisText, StringComparison.Ordinal);
        if (index == -1)
        {
            return string.Empty;
        }

        index += afterThisText.Length;

        if (index > uriPath.Length)
        {
            return string.Empty;
        }

        if (trimOffLeadingSlash && uriPath[index] == '/')
        {       
            index++;
        }

        var pathAfterText = uriPath.Substring(index);

        
        return pathAfterText;
    }
}
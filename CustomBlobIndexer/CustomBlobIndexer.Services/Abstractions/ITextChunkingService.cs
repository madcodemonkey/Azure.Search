namespace CustomBlobIndexer.Services;

public interface ITextChunkingService
{
    /// <summary>
    /// Breaks apart text for a document into the specified chunk size.
    /// </summary>
    /// <param name="allText">All the text that should be examined.</param>
    /// <param name="maximumChunkSize">The maximum size of any one chunk of text</param>
    /// <returns>An enumerable list of chunks.</returns>
    List<string> CreateChunks(string allText, int maximumChunkSize);
}
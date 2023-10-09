using System.Text;

namespace CustomBlobIndexer.Services;

public class TextChunkingService : ITextChunkingService
{
    private readonly char[] _sentenceEndings = { '.', '!', '?' };
    private readonly char[] _wordsBreaks = { ',', ';', ':', ' ', '(', ')', '[', ']', '{', '}', '\t', '\n' };
    private const int MaximumNumberOfCharactersBeforeBreakingOffAChunkOfALargeWord = 50;
 

    /// <summary>
    /// Breaks apart text for a document into the specified chunk size.
    /// </summary>
    /// <param name="allText">All the text that should be examined.</param>
    /// <param name="maximumChunkSize">The maximum size of any one chunk of text</param>
    /// <returns>An enumerable list of chunks.</returns>
    public List<string> CreateChunks(string allText, int maximumChunkSize)
    {
        var result = new List<string>();
        var sentences = BreakTextIntoSentences(allText);
        var currentChunk = new StringBuilder(); 
        var currentWords = new StringBuilder();

        foreach (string sentence in sentences)
        {
            if (currentChunk.Length + sentence.Length < maximumChunkSize)
            {
                AddTextToChunk(currentChunk,sentence);
                continue;
            }

            // At this point, the current sentence will not fit without breaking the maximumChunkSize limit!
            // So, we will break the current sentence down till it fits into a chunk that is smaller than maximumChunkSize.
            currentWords.Clear();
            currentWords.Append(sentence);

            // Deal with words in the current sentence until they are all gone by either shoving them in the
            // result list or the currentChunk.
            while (currentWords.Length > 0)
            {
                int maxCharThatCanBeAddedToCurrentChunk = maximumChunkSize - currentChunk.Length;
                int numberOfCharactersToAdd = FindNumberOfCharsThatWillGivesUsTheMostWords(currentWords, maxCharThatCanBeAddedToCurrentChunk);

                if (numberOfCharactersToAdd > 0)
                {
                    // Great, we can add stuff to the current chunk!!
                    AddTextToChunk(currentChunk, currentWords.ToString(0, numberOfCharactersToAdd));
                    currentWords = currentWords.Remove(0, numberOfCharactersToAdd);  // Shrink current words!

                    if (currentWords.Length <= maximumChunkSize)
                    {
                        AddChunkToList(result, currentChunk);
                        currentChunk.Append(currentWords.ToString());
                        currentWords.Clear();
                    }
                }
                else 
                {
                    // No words can be added to the current chunk.

                    if (currentChunk.Length == 0)
                    {
                        // The the current words is empty so just add characters up to the max since we are
                        // dealing with a mess of jumbled characters (e.g., didfi3904242sdf3w3f3f3rf3) 
                        AddTextToChunk(currentChunk, currentWords.ToString(0, maxCharThatCanBeAddedToCurrentChunk));
                        currentWords = currentWords.Remove(0, maxCharThatCanBeAddedToCurrentChunk);  // Shrink current words!

                        AddChunkToList(result, currentChunk);
                    }
                    else
                    {
                        // There is data already in the chunk, but the next few words will not fit.
                        AddChunkToList(result, currentChunk);
                    }
                }
            }

        }

        // Don't forget the last part of the last sentence.
        if (currentChunk.Length > 0)
        {
            AddChunkToList(result, currentChunk);
        }

        return result;
    }

    /// <summary>
    /// Adds one chunk to the chunk list.  It will also trim the chunk of text and exclude
    /// it if it was full of white spaces.  Finally, it will clear the chunk string builder.
    /// </summary>
    /// <param name="chunkList"></param>
    /// <param name="chunk"></param>
    private void AddChunkToList(List<string> chunkList, StringBuilder chunk)
    {
        var trimChunk = chunk.ToString().Trim();

        if (string.IsNullOrWhiteSpace(trimChunk) == false)
        {
            chunkList.Add(trimChunk);
        }

        chunk.Clear();
    }

    /// <summary>
    /// Adds text to a chunk.  If the chunk already has data within it, it will
    /// add a leading space so that the text doesn't bump up against another word.
    /// </summary>
    /// <param name="chunk"></param>
    /// <param name="textToAppend"></param>
    private void AddTextToChunk(StringBuilder chunk, string textToAppend)
    {
        if (chunk.Length > 0)
        {
            chunk.Append(" ");
        }

        chunk.Append(textToAppend);
    }

    /// <summary>
    /// Find the maximum count of characters that can be taken from the current words, but
    /// it will keep the number of characters under the proscribed maxCharThatCanBeAddedToCurrentChunk
    /// </summary>
    /// <param name="currentWords">The current work to examine</param>
    /// <param name="maxCharThatCanBeAddedToCurrentChunk">The maximum characters that can be returned.</param>
    /// <returns>The number of characters that should be taken to get whole words.</returns>
    private int FindNumberOfCharsThatWillGivesUsTheMostWords(StringBuilder currentWords, int maxCharThatCanBeAddedToCurrentChunk)
    {
        if (currentWords.Length == 0)
        {
            return 0;
        }

        if (currentWords.Length < maxCharThatCanBeAddedToCurrentChunk)
        {
            return currentWords.Length;
        }
        
        int resultLength = 0;
        int currentIndex = 0;
        int maxIndexPosition = maxCharThatCanBeAddedToCurrentChunk - 1;
        int charactersWithoutAWordBreak = 0;

        while (currentIndex < currentWords.Length && currentIndex < maxIndexPosition)
        {
            if (_wordsBreaks.Contains(currentWords[currentIndex]))
            {
                resultLength = currentIndex + 1;
                charactersWithoutAWordBreak = 0;
            }
            else
            {
                charactersWithoutAWordBreak++;
            }
            
            currentIndex++;
        }

        // If we hit a really super long section of text with no word breaks, we will just break off 
        // a chunk the size of the maxCharThatCanBeAddedToCurrentChunk because it's not a real word.
        if (charactersWithoutAWordBreak > MaximumNumberOfCharactersBeforeBreakingOffAChunkOfALargeWord)
        {
            resultLength = maxCharThatCanBeAddedToCurrentChunk;
        }


        return resultLength;
    }

    /// <summary>
    /// Breaks the text down into sentences based on the defined sentence ending punctuation. 
    /// </summary>
    /// <param name="allText">All the text</param>
    /// <returns>A list of sentences that are trimmed.</returns>
    private List<string> BreakTextIntoSentences(string allText)
    {
        var result = new List<string>();
        int currentIndex = 0;
        var currentSentence = new StringBuilder();

        while (currentIndex < allText.Length)
        {
            currentSentence.Append(allText[currentIndex]);
            if (_sentenceEndings.Contains(allText[currentIndex]))
            {
                var trimSentence = currentSentence.ToString().Trim();
                if (string.IsNullOrWhiteSpace(trimSentence) == false)
                {
                    result.Add(trimSentence);
                }
                currentSentence.Clear();
            }

            currentIndex++;
        }

        if (currentSentence.Length > 0)
        {
            var trimSentence = currentSentence.ToString().Trim();
            if (string.IsNullOrWhiteSpace(trimSentence) == false)
            {
                result.Add(trimSentence);
            }
        }
        
        return result;
    }
      
}
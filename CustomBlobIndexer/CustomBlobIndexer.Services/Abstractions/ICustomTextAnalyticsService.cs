using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomTextAnalyticsService
{
    /// <summary>
    /// Detect entities
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<SearchEntity>> DetectedEntitiesAsync(string input);

    Task<List<string>> DetectedKeyPhrases(string input);
    Task<List<SentenceSentiment>> DetectedSentiment(string input);

    /// <summary>
    /// Takes the content text and summarizes it into sentences.
    /// </summary>
    /// <param name="input">The content text</param>
    /// <returns>A list of sentences</returns>
    Task<List<ExtractiveSummarySentence>> ExtractSummarySentencesAsync(string input);

    /// <summary>
    /// Takes the content text and summarizes it into one sentence by calling <see cref="ExtractSummarySentencesAsync"/>
    /// and then appending all the sentences together.
    /// </summary>
    /// <param name="input">The content text</param>
    /// <returns>A single sentence</returns>
    Task<string> ExtractSummarySentenceAsync(string input);

    Task<string> RedactedText(string input);
    Task<List<SearchLanguage>> DetectLanguageInput(string input);
}
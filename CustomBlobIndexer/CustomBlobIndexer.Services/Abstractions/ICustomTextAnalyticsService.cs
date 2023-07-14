using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomTextAnalyticsService
{
    /// <summary>
    /// Detect entities
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    Task<List<SearchEntity>> DetectedEntitiesAsync(string content);

    Task<List<string>> DetectedKeyPhrases(string content);
    Task<List<SentenceSentiment>> DetectedSentiment(string content);

    /// <summary>
    /// Takes the content text and summarizes it into sentences.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A list of sentences</returns>
    Task<List<ExtractiveSummarySentence>> ExtractSummarySentencesAsync(string content);

    /// <summary>
    /// Takes the content text and summarizes it into one sentence by calling <see cref="ExtractSummarySentencesAsync"/>
    /// and then appending all the sentences together.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A single sentence</returns>
    Task<string> ExtractSummarySentenceAsync(string content);

    Task<string> RedactedText(string content);
    Task<List<SearchLanguage>> DetectLanguageInput(string content);
}
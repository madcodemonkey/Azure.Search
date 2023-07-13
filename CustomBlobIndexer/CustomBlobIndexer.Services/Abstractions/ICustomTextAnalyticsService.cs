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
    Task<List<Entity>> DetectedEntitiesAsync(string input);

    Task<List<string>> DetectedKeyPhrases(string input);
    Task<List<SentenceSentiment>> DetectedSentiment(string input);
    Task<List<ExtractiveSummarySentence>> ExtractSummaryResultsAsync(string input);


    Task<string> RedactedText(string input);
    Task<List<Language>> DetectLanguageInput(string input);
}
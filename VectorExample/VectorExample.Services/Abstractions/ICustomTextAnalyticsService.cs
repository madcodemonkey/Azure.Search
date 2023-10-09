using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public interface ICustomTextAnalyticsService
{
    /// <summary>
    /// Named Entity Recognition (NER) - it identifies and categorizes entities in unstructured
    /// text. For example: people, places, organizations, and quantities. You will have to examine
    /// the category and sub-category in order to determine the entity type.
    /// </summary>
    /// <param name="content">The text to analyze for entities.</param>
    /// <returns></returns>
    /// <remarks>
    /// Docs:  https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/named-entity-recognition/overview
    /// Example: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample4_RecognizeEntities.md
    /// </remarks>
    Task<List<SearchEntity>> DetectedEntitiesAsync(string content);

    /// <summary>
    /// Use key phrase extraction to quickly identify the main concepts in text. For example, in the
    /// text "The food was delicious and the staff were wonderful.", key phrase extraction will
    /// return the main topics: "food" and "wonderful staff".
    /// </summary>
    /// <param name="content">The text to analyze for key phrases.</param>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/key-phrase-extraction/overview
    /// Example:  https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample3_ExtractKeyPhrases.md
    /// </remarks>
    Task<List<string>> DetectedKeyPhrases(string content);

    /// <summary>
    /// Sentiment detection provides sentiment labels (such as "negative", "neutral" and "positive")
    /// based on the highest confidence score found by the service at a SENTENCE and DOCUMENT level;
    /// however, this method is only returning sentence sentiment. It has document sentiment
    /// available as well, but isn't returning that data.
    /// </summary>
    /// <param name="content">
    /// The content to be analyzed (usually the whole document, which it will chunk through)
    /// </param>
    /// <returns>A list of sentence sentiments</returns>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/sentiment-opinion-mining/overview
    /// Example 1: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample2_AnalyzeSentiment.md
    /// Example 2: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample2.1_AnalyzeSentimentWithOpinionMining.md
    /// </remarks>
    Task<List<SentenceSentiment>> DetectedSentiment(string content);

    /// <summary>
    /// Attempts to detect languages in the document and will give the language and a confidence score.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/language-detection/overview
    /// Example: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample1_DetectLanguage.md
    /// </remarks>
    Task<List<SearchLanguage>> DetectLanguageInput(string content);

    /// <summary>
    /// Takes the content text and summarizes it into one sentence by calling <see
    /// cref="ExtractSummarySentencesAsync"/> and then appending all the sentences together.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A single sentence</returns>
    Task<string> ExtractSummarySentenceAsync(string content);

    /// <summary>
    /// Takes the content text and summarizes it into sentences.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A list of sentences</returns>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/summarization/overview?tabs=document-summarization
    /// Examples:
    /// https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample11_ExtractiveSummarize.md https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample12_AbstractiveSummarize.md
    /// </remarks>
    Task<List<ExtractiveSummarySentence>> ExtractSummarySentencesAsync(string content);

    /// <summary>
    /// Runs a predictive model to identify a collection of entities containing Personally
    /// Identifiable Information found in the passed-in document, and categorize those entities into
    /// types such as US social security number, drivers license number, or credit card number.
    /// Note that this method is only returning the redacted text, but it if you examine the commented
    /// out code carefully you will notice that the categorized data is there as well.
    /// </summary>
    /// <param name="content">The content text to examine</param>
    /// <returns>Redacted text</returns>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/personally-identifiable-information/overview
    /// Sample 5: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample5_RecognizePiiEntities.md
    /// </remarks>
    Task<string> RedactedText(string content);
}
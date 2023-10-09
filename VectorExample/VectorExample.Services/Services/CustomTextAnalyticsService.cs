using Azure;
using Azure.AI.TextAnalytics;
using VectorExample.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

// TODO: Remove all Console.WriteLines

public class CustomTextAnalyticsService : ICustomTextAnalyticsService
{
    private static TextAnalyticsClient? _client;
    private readonly ILogger<CustomTextAnalyticsService> _logger;
    private readonly CognitiveServiceSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomTextAnalyticsService(ILogger<CustomTextAnalyticsService> logger, IOptions<CognitiveServiceSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

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
    public async Task<List<SearchEntity>> DetectedEntitiesAsync(string content)
    {
        var client = GetClient();

        var entityList = new List<SearchEntity>();

        var chunks = ChunksUpto(content, 5120);

        foreach (var chunk in chunks)
        {
            Response<CategorizedEntityCollection> response = await client.RecognizeEntitiesAsync(chunk);
            CategorizedEntityCollection entitiesInDocument = response.Value;

            // Console.WriteLine($"Recognized {entitiesInDocument.Count} entities:");
            foreach (CategorizedEntity entity in entitiesInDocument)
            {
                SearchEntity e = new SearchEntity
                {
                    Category = (string)entity.Category,
                    Subcategory = entity.SubCategory,
                    Text = entity.Text
                };

                if (entityList.Count(ent => ent.Text == e.Text) == 0)
                {
                    entityList.Add(e);
                    //Console.WriteLine($"  Text: {entity.Text}");
                    //Console.WriteLine($"  Offset: {entity.Offset}");
                    //Console.WriteLine($"  Length: {entity.Length}");
                    //Console.WriteLine($"  Category: {entity.Category}");
                    //if (!string.IsNullOrEmpty(entity.SubCategory))
                    //    Console.WriteLine($"  SubCategory: {entity.SubCategory}");
                    //Console.WriteLine($"  Confidence score: {entity.ConfidenceScore}");
                    //Console.WriteLine("");
                }
            }
        }

        return entityList;
    }

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
    public async Task<List<string>> DetectedKeyPhrases(string content)
    {
        var client = GetClient();

        var keyPhraseList = new List<string>();

        var chunks = ChunksUpto(content, 5120);

        foreach (var chunk in chunks)
        {
            Response<KeyPhraseCollection> response = await client.ExtractKeyPhrasesAsync(chunk);
            KeyPhraseCollection keyPhrases = response.Value;

            // Console.WriteLine($"Extracted {keyPhrases.Count} key phrases:");
            foreach (string keyPhrase in keyPhrases)
            {
                if (!keyPhraseList.Contains(keyPhrase))
                {
                    keyPhraseList.Add(keyPhrase);
                    // Console.WriteLine($" {keyPhrase}");
                }
            }
        }

        return keyPhraseList;
    }

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
    public async Task<List<SentenceSentiment>> DetectedSentiment(string content)
    {
        var client = GetClient();

        List<SentenceSentiment> sentiments = new List<SentenceSentiment>();

        var chunks = ChunksUpto(content, 5120);

        foreach (var chunk in chunks)
        {
            Response<DocumentSentiment> response = await client.AnalyzeSentimentAsync(chunk);
            foreach (SentenceSentiment sentenceSentiment in response.Value.Sentences)
            {
                sentiments.Add(sentenceSentiment);
            }
        }

        return sentiments;
    }

    /// <summary>
    /// Attempts to detect languages in the document and will give the language and a confidence score.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <remarks>
    /// Docs: https://learn.microsoft.com/en-us/azure/cognitive-services/language-service/language-detection/overview
    /// Example: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample1_DetectLanguage.md
    /// </remarks>
    public async Task<List<SearchLanguage>> DetectLanguageInput(string content)
    {
        var client = GetClient();

        List<SearchLanguage> languages = new List<SearchLanguage>();

        var chunks = ChunksUpto(content, 5120);

        foreach (var chunk in chunks)
        {
            Response<DetectedLanguage> response = await client.DetectLanguageAsync(chunk);
            SearchLanguage l = new SearchLanguage();
            l.Confidence = response.Value.ConfidenceScore;
            l.Name = response.Value.Name;
            l.Iso6391Name = response.Value.Iso6391Name;
            if (languages.Count(lang => lang.Iso6391Name != l.Iso6391Name) == 0)
            {
                languages.Add(l);
            }
            // Console.WriteLine($"Detected language {response.Value.Name} with confidence score {response.Value.ConfidenceScore}.");
        }

        return languages;
    }

    /// <summary>
    /// Takes the content text and summarizes it into one sentence by calling <see
    /// cref="ExtractSummarySentencesAsync"/> and then appending all the sentences together.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A single sentence</returns>
    public async Task<string> ExtractSummarySentenceAsync(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length < 50)
            return content;

        // TODO: When sending in short text (e.g., '1964\r\nS-E-CAR-E-T') I saw an unhelpful exception.  Hidden characters?  Too short?  
        try
        {
            var sentenceList = await ExtractSummarySentencesAsync(content);

            StringBuilder sb = new StringBuilder();

            foreach (ExtractiveSummarySentence s in sentenceList)
            {
                sb.Append(s.Text);
                sb.AppendLine(" ...");
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            string message = "Unable to summarize text!";
            _logger.LogError(ex, $"{message}: {content.Substring(0, content.Length > 100 ? 100 : content.Length)}...");
            return message;
        }
    }

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
    public async Task<List<ExtractiveSummarySentence>> ExtractSummarySentencesAsync(string content)
    {
        var client = GetClient();

        List<ExtractiveSummarySentence> summaryList = new List<ExtractiveSummarySentence>();

        var chunks = ChunksUpto(content, 125000);

        foreach (string chunk in chunks)
        {
            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractiveSummarizeActions = new List<ExtractiveSummarizeAction>()
                        { new ExtractiveSummarizeAction() }
            };

            var doc = new List<string>();
            doc.Add(chunk);
            var operation = await client.StartAnalyzeActionsAsync(doc, actions);
            await operation.WaitForCompletionAsync();
            await foreach (AnalyzeActionsResult documentsInPage in operation.Value)
            {
                IReadOnlyCollection<ExtractiveSummarizeActionResult> summaryResults =
                    documentsInPage.ExtractiveSummarizeResults;

                foreach (ExtractiveSummarizeActionResult summaryActionResults in summaryResults)
                {
                    if (summaryActionResults.HasError)
                    {
                        _logger.LogError(
                            $"  Action error code: {summaryActionResults.Error.ErrorCode}.  Message: {summaryActionResults.Error.Message}");
                        //Console.WriteLine($"  Error!");
                        //Console.WriteLine($"  Action error code: {summaryActionResults.Error.ErrorCode}.");
                        //Console.WriteLine($"  Message: {summaryActionResults.Error.Message}");
                        continue;
                    }

                    foreach (ExtractiveSummarizeResult documentResults in summaryActionResults.DocumentsResults)
                    {
                        if (documentResults.HasError)
                        {
                            _logger.LogError(
                                $"  Document error code: {documentResults.Error.ErrorCode}.   Message: {documentResults.Error.Message}");
                            //Console.WriteLine($"  Error!");
                            //Console.WriteLine($"  Document error code: {documentResults.Error.ErrorCode}.");
                            //Console.WriteLine($"  Message: {documentResults.Error.Message}");
                            continue;
                        }

                        //Console.WriteLine($"  Extracted the following {documentResults.Sentences.Count} sentence(s):");
                        //Console.WriteLine();

                        foreach (ExtractiveSummarySentence sentence in documentResults.Sentences)
                        {
                            summaryList.Add(sentence);
                            //Console.WriteLine($"  Sentence: {sentence.Text}");
                            //Console.WriteLine();
                        }
                    }
                }
            }
        }

        return summaryList;

    }

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
    public async Task<string> RedactedText(string content)
    {
        var client = GetClient();

        var sb = new StringBuilder();

        var chunks = ChunksUpto(content, 5120);

        foreach (var chunk in chunks)
        {
            Response<PiiEntityCollection> response = await client.RecognizePiiEntitiesAsync(chunk);
            PiiEntityCollection entities = response.Value;
            sb.Append(entities.RedactedText);

            //Console.WriteLine($"Redacted Text: {entities.RedactedText}");
            //Console.WriteLine("");
            //Console.WriteLine($"Recognized {entities.Count} PII entities:");
            //foreach (PiiEntity entity in entities)
            //{
            //    Console.WriteLine($"  Text: {entity.Text}");
            //    Console.WriteLine($"  Category: {entity.Category}");
            //    if (!string.IsNullOrEmpty(entity.SubCategory))
            //        Console.WriteLine($"  SubCategory: {entity.SubCategory}");
            //    Console.WriteLine($"  Confidence score: {entity.ConfidenceScore}");
            //    Console.WriteLine("");
            //}
        }

        return sb.ToString();
    }

    private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += maxChunkSize)
            yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
    }

    private TextAnalyticsClient GetClient()
    {
        return _client ??= new TextAnalyticsClient(new Uri(_settings.Endpoint),
            new AzureKeyCredential(_settings.Key));
    }
}
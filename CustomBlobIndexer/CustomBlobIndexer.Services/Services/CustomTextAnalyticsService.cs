using System.Text;
using Azure;
using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer.Services;

// TODO: Remove all Console.WriteLines

public class CustomTextAnalyticsService : ICustomTextAnalyticsService
{
    private static TextAnalyticsClient? _client;
    private readonly ILogger<CustomTextAnalyticsService> _logger;
    private readonly ServiceSettings _settings;
    /// <summary>
    /// Constructor 
    /// </summary>
    public CustomTextAnalyticsService(ILogger<CustomTextAnalyticsService> logger, ServiceSettings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    /// <summary>
    /// Detect entities
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
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
                    // Console.WriteLine($"  {keyPhrase}");
                }
            }
        }
        
        return keyPhraseList;
    }

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
    /// Takes the content text and summarizes it into sentences.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A list of sentences</returns>
    public async Task<List<ExtractiveSummarySentence>> ExtractSummarySentencesAsync(string content)
    {
        var client = GetClient();

        List<ExtractiveSummarySentence> summaryList = new List<ExtractiveSummarySentence>();


        var chunks = ChunksUpto(content, 125000);

        foreach (string chunk in chunks)
        {
            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractiveSummarizeActions = new List<ExtractiveSummarizeAction>() { new ExtractiveSummarizeAction() }
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
                        _logger.LogError($"  Action error code: {summaryActionResults.Error.ErrorCode}.  Message: {summaryActionResults.Error.Message}");
                        //Console.WriteLine($"  Error!");
                        //Console.WriteLine($"  Action error code: {summaryActionResults.Error.ErrorCode}.");
                        //Console.WriteLine($"  Message: {summaryActionResults.Error.Message}");
                        continue;
                    }

                    foreach (ExtractiveSummarizeResult documentResults in summaryActionResults.DocumentsResults)
                    {
                        if (documentResults.HasError)
                        {
                            _logger.LogError($"  Document error code: {documentResults.Error.ErrorCode}.   Message: {documentResults.Error.Message}");
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
    /// Takes the content text and summarizes it into one sentence by calling <see cref="ExtractSummarySentencesAsync"/>
    /// and then appending all the sentences together.
    /// </summary>
    /// <param name="content">The content text</param>
    /// <returns>A single sentence</returns>
    public async Task<string> ExtractSummarySentenceAsync(string content)
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

    /// <summary>
    /// Runs a predictive model to identify a collection of entities containing Personally
    /// Identifiable Information found in the passed-in document, and categorize those entities
    /// into types such as US social security number, drivers license number, or credit card number.
    /// </summary>
    /// <param name="content">The content text to examine</param>
    /// <returns>Redacted text</returns>
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

    private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
    {
        for (int i = 0; i < str.Length; i += maxChunkSize)
            yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
    }

    private TextAnalyticsClient GetClient()
    {
        return _client ??= new TextAnalyticsClient(new Uri(_settings.CognitiveServiceEndpoint),
            new AzureKeyCredential(_settings.CognitiveServiceKey));
    }
}
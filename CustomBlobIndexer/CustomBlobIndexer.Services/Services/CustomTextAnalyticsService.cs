using Azure;
using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

// TODO: Remove all Console.WriteLines

public class CustomTextAnalyticsService : ICustomTextAnalyticsService
{
    private static TextAnalyticsClient? _client;
    private readonly ServiceSettings _settings;
    /// <summary>
    /// Constructor 
    /// </summary>
    public CustomTextAnalyticsService(ServiceSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Detect entities
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<Entity>> DetectedEntitiesAsync(string input)
    {
        var client = GetClient();

        List<Entity> entityList = new List<Entity>();

        try
        {
            var chunks = ChunksUpto(input, 5120);

            foreach (var chunk in chunks)
            {
                Response<CategorizedEntityCollection> response = await client.RecognizeEntitiesAsync(chunk);
                CategorizedEntityCollection entitiesInDocument = response.Value;

                Console.WriteLine($"Recognized {entitiesInDocument.Count} entities:");
                foreach (CategorizedEntity entity in entitiesInDocument)
                {
                    Entity e = new Entity();
                    e.Category = ((string)entity.Category);
                    e.Subcategory = entity.SubCategory;
                    e.Text = entity.Text;
                    if (entityList.Where(ent => ent.Text == e.Text).Count() == 0)
                    {
                        entityList.Add(e);
                        Console.WriteLine($"  Text: {entity.Text}");
                        Console.WriteLine($"  Offset: {entity.Offset}");
                        Console.WriteLine($"  Length: {entity.Length}");
                        Console.WriteLine($"  Category: {entity.Category}");
                        if (!string.IsNullOrEmpty(entity.SubCategory))
                            Console.WriteLine($"  SubCategory: {entity.SubCategory}");
                        Console.WriteLine($"  Confidence score: {entity.ConfidenceScore}");
                        Console.WriteLine("");
                    }
                }
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        return entityList;
    }

    public async Task<List<string>> DetectedKeyPhrases(string input)
    {
        var client = GetClient();

        List<string> keyPhraseList = new List<string>();

        try
        {
            var chunks = ChunksUpto(input, 5120);

            foreach (var chunk in chunks)
            {
                Response<KeyPhraseCollection> response = await client.ExtractKeyPhrasesAsync(chunk);
                KeyPhraseCollection keyPhrases = response.Value;

                Console.WriteLine($"Extracted {keyPhrases.Count} key phrases:");
                foreach (string keyPhrase in keyPhrases)
                {
                    if (!keyPhraseList.Contains(keyPhrase))
                    {
                        keyPhraseList.Add(keyPhrase);
                        Console.WriteLine($"  {keyPhrase}");
                    }
                }
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        return keyPhraseList;
    }

    public async Task<List<SentenceSentiment>> DetectedSentiment(string input)
    {
        var client = GetClient();

        List<SentenceSentiment> sentiments = new List<SentenceSentiment>();

        try
        {
            var chunks = ChunksUpto(input, 5120);

            foreach (var chunk in chunks)
            {
                Response<DocumentSentiment> response = await client.AnalyzeSentimentAsync(chunk);
                foreach (SentenceSentiment sentenceSentiment in response.Value.Sentences)
                {
                    sentiments.Add(sentenceSentiment);
                }
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        return sentiments;
    }

    public async Task<List<ExtractiveSummarySentence>> ExtractSummaryResultsAsync(string input)
    {
        var client = GetClient();

        List<ExtractiveSummarySentence> summaryList = new List<ExtractiveSummarySentence>();

        try
        {
            var chunks = ChunksUpto(input, 125000);

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
                    IReadOnlyCollection<ExtractiveSummarizeActionResult> summaryResults = documentsInPage.ExtractiveSummarizeResults;

                    foreach (ExtractiveSummarizeActionResult summaryActionResults in summaryResults)
                    {
                        if (summaryActionResults.HasError)
                        {
                            Console.WriteLine($"  Error!");
                            Console.WriteLine($"  Action error code: {summaryActionResults.Error.ErrorCode}.");
                            Console.WriteLine($"  Message: {summaryActionResults.Error.Message}");
                            continue;
                        }

                        foreach (ExtractiveSummarizeResult documentResults in summaryActionResults.DocumentsResults)
                        {
                            if (documentResults.HasError)
                            {
                                Console.WriteLine($"  Error!");
                                Console.WriteLine($"  Document error code: {documentResults.Error.ErrorCode}.");
                                Console.WriteLine($"  Message: {documentResults.Error.Message}");
                                continue;
                            }

                            Console.WriteLine($"  Extracted the following {documentResults.Sentences.Count} sentence(s):");
                            Console.WriteLine();

                            foreach (ExtractiveSummarySentence sentence in documentResults.Sentences)
                            {
                                summaryList.Add(sentence);
                                Console.WriteLine($"  Sentence: {sentence.Text}");
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        return summaryList;
    }

    public async Task<string> RedactedText(string input)
    {
        var client = GetClient();

        // List<PiiEntityCollection> piiList = new List<PiiEntityCollection>();
        string redactedText = "";

        try
        {
            var chunks = ChunksUpto(input, 5120);

            foreach (var chunk in chunks)
            {
                Response<PiiEntityCollection> response = await client.RecognizePiiEntitiesAsync(chunk);
                PiiEntityCollection entities = response.Value;
                // piiList.Add(entities);
                redactedText += entities.RedactedText;

                Console.WriteLine($"Redacted Text: {entities.RedactedText}");
                Console.WriteLine("");
                Console.WriteLine($"Recognized {entities.Count} PII entities:");
                foreach (PiiEntity entity in entities)
                {
                    Console.WriteLine($"  Text: {entity.Text}");
                    Console.WriteLine($"  Category: {entity.Category}");
                    if (!string.IsNullOrEmpty(entity.SubCategory))
                        Console.WriteLine($"  SubCategory: {entity.SubCategory}");
                    Console.WriteLine($"  Confidence score: {entity.ConfidenceScore}");
                    Console.WriteLine("");
                }
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
        }

        return redactedText;
    }

    public async Task<List<Language>> DetectLanguageInput(string input)
    {
        var client = GetClient();

        List<Language> languages = new List<Language>();

        try
        {
            var chunks = ChunksUpto(input, 5120);

            foreach (var chunk in chunks)
            {
                Response<DetectedLanguage> response = await client.DetectLanguageAsync(chunk);
                Language l = new Language();
                l.Confidence = response.Value.ConfidenceScore;
                l.Name = response.Value.Name;
                l.Iso6391Name = response.Value.Iso6391Name;
                if (languages.Where(lang => lang.Iso6391Name != l.Iso6391Name).Count() == 0)
                {
                    languages.Add(l);
                }
                // Console.WriteLine($"Detected language {response.Value.Name} with confidence score {response.Value.ConfidenceScore}.");
            }
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
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
using Azure.AI.TextAnalytics;
using CustomBlobIndexer.Models;
using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer;

public class BlobTriggerFunction
{
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ICustomComputerVisionService _computerVisionService;
    private readonly ICustomTextAnalyticsService _textAnalyticsService;
    private readonly ICustomSearchIndexService _searchIndexService;
    private readonly ILogger _logger;

    public BlobTriggerFunction(ILoggerFactory loggerFactory,
        IBlobSasBuilderService sasBuilderService, 
        ICustomComputerVisionService computerVisionService,
        ICustomTextAnalyticsService textAnalyticsService,
        ICustomSearchIndexService searchIndexService)
    {
        _sasBuilderService = sasBuilderService;
        _computerVisionService = computerVisionService;
        _textAnalyticsService = textAnalyticsService;
        _searchIndexService = searchIndexService;
        _logger = loggerFactory.CreateLogger<BlobTriggerFunction>();
    }

      
    [Function("BlobTriggerFunction")]
    public async Task Run([BlobTrigger("my-files/{name}", Connection = "BlobStorageConnectionString")] byte[] myBlob, string name, Uri uri)
    {
        try
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} Uri: {uri}  ");
            
            string sasUrl = _sasBuilderService.GenerateSaSUrl(name, uri);
            
            _logger.LogInformation(sasUrl);

            Document d = new Document();
            d.Id = Base64EncodeString(uri.ToString());
            d.Content = await _computerVisionService.ReadFileAsync(sasUrl);
            d.Title = name;
            d.Source = "blob";

            // Call Cognitive Services  for enrichment (skillset replacement)
            d.KeyPhrases = await _textAnalyticsService.DetectedKeyPhrases(d.Content);
            d.Languages = await _textAnalyticsService.DetectLanguageInput(d.Content);
            d.Entities = await _textAnalyticsService.DetectedEntitiesAsync(d.Content);
            // d.Sentiments = await _textAnalyticsService.DetectedSentiment(d.Content);
            // d.RedactedText = await _textAnalyticsService.RedactedText(d.Content);

            var summary = await _textAnalyticsService.ExtractSummaryResultsAsync(d.Content);
            d.Summary = "";
            foreach (ExtractiveSummarySentence s in summary)
            {
                d.Summary += s.Text + " ...\n\n";
            }

            // Index enriched content in Cognitive Search
            // _searchIndexService.CreateOrUpdateIndex();
            _searchIndexService.UploadDocuments(d);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something bad happened");
        }
    }

    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
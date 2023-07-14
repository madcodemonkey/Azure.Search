using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Logging;
using System;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

public class FileProcessService : IFileProcessService
{
    private readonly ServiceSettings _settings;
    private readonly ILogger<FileProcessService> _logger;
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ICustomComputerVisionService _computerVisionService;
    private readonly ICustomTextAnalyticsService _textAnalyticsService;
    private readonly ICustomSearchIndexService _searchIndexService;
     

    /// <summary>
    /// Constructor
    /// </summary>
    public FileProcessService(ServiceSettings settings, 
        ILogger<FileProcessService> logger,
        IBlobSasBuilderService sasBuilderService,
        ICustomComputerVisionService computerVisionService,
        ICustomTextAnalyticsService textAnalyticsService,
        ICustomSearchIndexService searchIndexService)
    {
        _settings = settings;
        _logger = logger;
        _sasBuilderService = sasBuilderService;
        _computerVisionService = computerVisionService;
        _textAnalyticsService = textAnalyticsService;
        _searchIndexService = searchIndexService;
    }

    public async Task ProcessFileAsync(string name, Uri uri)
    {
        _logger.LogInformation($"Process file named: {name} located a uri: {uri}");

        string sasUrl = _sasBuilderService.GenerateSaSUrl(name, uri);

        _logger.LogInformation(sasUrl);

        var d = new Document
        {
            Id = Base64EncodeString(uri.ToString()),
            Content = await _computerVisionService.ReadFileAsync(sasUrl),
            Title = name,
            Source = "blob"
        };

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

        
        _searchIndexService.UploadDocuments(d);
    }

    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
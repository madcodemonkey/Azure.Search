using Microsoft.Extensions.Logging;
using CustomBlobIndexer.Models;

namespace CustomBlobIndexer.Services;

/// <summary>
/// Processes a file and creates ONE search document in the Azure Cognitive Search index.
/// </summary>
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

        string content = await _computerVisionService.ReadFileAsync(sasUrl);

        var d = new SearchIndexDocument
        {
            Id = Base64EncodeString(uri.ToString()),
            Content = content,
            Title = name,
            Source = "blob",
            Summary = await _textAnalyticsService.ExtractSummarySentenceAsync(content)
        };

        
        if (_settings.CognitiveSearchSkillDetectKeyPhrases)
        {
            d.KeyPhrases = await _textAnalyticsService.DetectedKeyPhrases(content);
        }

        if (_settings.CognitiveSearchSkillDetectLanguage)
        {
            d.Languages = await _textAnalyticsService.DetectLanguageInput(content);
        }

        if (_settings.CognitiveSearchSkillDetectEntities)
        {
            d.Entities = await _textAnalyticsService.DetectedEntitiesAsync(content);
        }

        if (_settings.CognitiveSearchSkillDetectSentiment)
        {
            //d.Sentiments = await _textAnalyticsService.DetectedSentiment(content);
        }

        if (_settings.CognitiveSearchSkillRedactText)
        {
            d.RedactedText = await _textAnalyticsService.RedactedText(content);
        }


        _searchIndexService.UploadDocuments(d);
    }

    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
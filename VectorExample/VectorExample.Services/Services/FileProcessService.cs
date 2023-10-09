using CogSimple.Services;
using Microsoft.Extensions.Logging;
using VectorExample.Models;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

/// <summary>
/// Processes a file and creates ONE search document in the Azure Cognitive Search index.
/// </summary>
public class FileProcessService : IFileProcessService
{
    private readonly ApplicationSettings _appSettings;
    private readonly BlobSettings _blobSettings;
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ICogSearchIndexService _cogSearchIndexService;
    private readonly ICustomComputerVisionService _computerVisionService;
    private readonly ICustomTextAnalyticsService _textAnalyticsService;
    private readonly ILogger<FileProcessService> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public FileProcessService(IOptions<BlobSettings> blobSettings, IOptions<ApplicationSettings> appSettings,
        ILogger<FileProcessService> logger,
        IBlobSasBuilderService sasBuilderService,
        ICustomComputerVisionService computerVisionService,
        ICogSearchIndexService cogSearchIndexService,
        ICustomTextAnalyticsService textAnalyticsService)
    {
        _appSettings = appSettings.Value;
        _blobSettings = blobSettings.Value;
        _logger = logger;
        _sasBuilderService = sasBuilderService;
        _computerVisionService = computerVisionService;
        _cogSearchIndexService = cogSearchIndexService;
        _textAnalyticsService = textAnalyticsService;
    }

    public async Task ProcessFileAsync(string name, Uri uri, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Process file named: {name} located a uri: {uri}");

        string sasUrl = _sasBuilderService.GenerateSaSUrl(name, uri);

        _logger.LogInformation(sasUrl);

        string content = await _computerVisionService.ReadFileAsync(sasUrl);
        var sourcePath = uri.GetPathAfterText(_blobSettings.ContainerName);
  
        var d = new SearchIndexDocument
        {
            Id = Base64EncodeString(sourcePath),
            ChunkOrderNumber = 1, // Since we are not chunking, there is only one number
            Content = content,
            SourcePath = sourcePath,
            Summary = _appSettings.CognitiveSearchSkillSummarizeText ?
                await _textAnalyticsService.ExtractSummarySentenceAsync(content) : string.Empty,
            Title = Path.GetFileName(name)
        };

        d.KeyPhrases = _appSettings.CognitiveSearchSkillDetectKeyPhrases ?
            await _textAnalyticsService.DetectedKeyPhrases(content) :
            new List<string>();

        d.Languages = _appSettings.CognitiveSearchSkillDetectLanguage ?
            await _textAnalyticsService.DetectLanguageInput(content) :
            new List<SearchLanguage>();


        d.Entities = _appSettings.CognitiveSearchSkillDetectEntities ?
            await _textAnalyticsService.DetectedEntitiesAsync(content) :
            new List<SearchEntity>();

 

        if (_appSettings.CognitiveSearchSkillDetectSentiment)
        {
            //d.Sentiments = await _textAnalyticsService.DetectedSentiment(content);
        }

        if (_appSettings.CognitiveSearchSkillRedactText)
        {
            d.RedactedText = await _textAnalyticsService.RedactedText(content);
        }


        await _cogSearchIndexService.UploadDocumentsAsync(_appSettings.CognitiveSearchIndexName, d, cancellationToken);
    }

    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
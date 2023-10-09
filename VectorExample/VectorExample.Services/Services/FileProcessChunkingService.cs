using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using VectorExample.Models;
using Microsoft.Extensions.Logging;
using CogSimple.Services;
using Microsoft.Extensions.Options;

namespace VectorExample.Services;

/// <summary>
/// Processes a file and creates MULTIPLE search documents in the Azure Cognitive Search index based on the chunk size.
/// This chunking is done to limit the size of results we submit to Open AI.
/// </summary>
public class FileProcessChunkingService : IFileProcessService
{
    private readonly ApplicationSettings _appSettings;
    private readonly BlobSettings _blobSettings;
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ICogSearchIndexService _cogSearchIndexService;
    private readonly ICustomComputerVisionService _computerVisionService;
    private readonly ICustomTextAnalyticsService _textAnalyticsService;
    private readonly ILogger<FileProcessService> _logger;
    private readonly ITextChunkingService _textChunkingService;

    /// <summary>
    /// Constructor
    /// </summary>
    public FileProcessChunkingService(IOptions<BlobSettings> blobSettings, IOptions<ApplicationSettings> appSettings,
        ILogger<FileProcessService> logger,
        IBlobSasBuilderService sasBuilderService,
        ICustomComputerVisionService computerVisionService,
        ICustomTextAnalyticsService textAnalyticsService,
        ICogSearchIndexService cogSearchIndexService,
        ITextChunkingService textChunkingService)
    {
        _appSettings = appSettings.Value;
        _blobSettings = blobSettings.Value;
        _logger = logger;
        _sasBuilderService = sasBuilderService;
        _computerVisionService = computerVisionService;
        _textAnalyticsService = textAnalyticsService;
        _cogSearchIndexService = cogSearchIndexService;
        _textChunkingService = textChunkingService;
    }

    public async Task ProcessFileAsync(string name, Uri uri, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Process file named: {name} located a uri: {uri} into chunks");

        string sasUrl = _sasBuilderService.GenerateSaSUrl(name, uri);

        _logger.LogInformation(sasUrl);

        string content = await _computerVisionService.ReadFileAsync(sasUrl);
        List<string> contentChunks = _textChunkingService.CreateChunks(content, _appSettings.ChunkMaximumNumberOfCharacters);
        var sourcePath = uri.GetPathAfterText(_blobSettings.ContainerName);

        int chunkOrderNumber = 1;
        foreach (var contentChunk in contentChunks)
        {
            var id = Base64EncodeString($"{chunkOrderNumber}/{sourcePath}");

            var d = new SearchIndexDocument
            {
                Id = id,
                ChunkOrderNumber = chunkOrderNumber++, // allows you to put the chunks back together in the proper order.
                Content = contentChunk,
                SourcePath = uri.GetPathAfterText(_blobSettings.ContainerName),
                Summary = _appSettings.CognitiveSearchSkillSummarizeText ?
                    await _textAnalyticsService.ExtractSummarySentenceAsync(contentChunk) : string.Empty,
                Title = Path.GetFileName(name)
            };

            d.KeyPhrases = _appSettings.CognitiveSearchSkillDetectKeyPhrases ? 
                await _textAnalyticsService.DetectedKeyPhrases(contentChunk) : 
                new List<string>();

            d.Languages = _appSettings.CognitiveSearchSkillDetectLanguage ? 
                await _textAnalyticsService.DetectLanguageInput(contentChunk) :
                new List<SearchLanguage>();
            

            d.Entities = _appSettings.CognitiveSearchSkillDetectEntities ? 
                await _textAnalyticsService.DetectedEntitiesAsync(contentChunk) :
                new List<SearchEntity>();
            

            if (_appSettings.CognitiveSearchSkillDetectSentiment)
            {
                //d.Sentiments = await _textAnalyticsService.DetectedSentiment(contentChunk);
            }

            if (_appSettings.CognitiveSearchSkillRedactText)
            {
                d.RedactedText = await _textAnalyticsService.RedactedText(contentChunk);
            }

            await _cogSearchIndexService.UploadDocumentsAsync(_appSettings.CognitiveSearchIndexName, d, cancellationToken);
        }

        await DeleteAnyRemainingChunksAsync(chunkOrderNumber, sourcePath, cancellationToken);
    }

    /// <summary>
    /// If we are overwriting an existing document, it's possible that it has fewer chunks than the last time
    /// it was broken apart.  We should deleting those remaining chunks.
    /// </summary>
    /// <param name="chunkOrderNumber">The first chunk number that should be deleted.</param>
    /// <param name="sourcePath">The name and partial path to the file.  This should be unique if our data is coming out of ONE blob storage container.</param>
    /// <returns></returns>
    private async Task DeleteAnyRemainingChunksAsync(int chunkOrderNumber, string sourcePath, CancellationToken cancellationToken = default)
    {
        var options = new SearchOptions
        {
            QueryType = SearchQueryType.Simple,
            IncludeTotalCount = true,
            Select = { nameof(SearchIndexDocument.Id) },
            Filter = $"{nameof(SearchIndexDocument.SourcePath)} eq '{sourcePath}' and {nameof(SearchIndexDocument.ChunkOrderNumber)} ge {chunkOrderNumber}"
        };
        
        var result = await _cogSearchIndexService.SearchAsync<SearchIndexDocument>(_appSettings.CognitiveSearchIndexName, "*", options, cancellationToken);

        List<string> keys = result.Docs.Select(s => s.Document.Id).ToList();
        
        await _cogSearchIndexService.DeleteDocumentsAsync(_appSettings.CognitiveSearchIndexName, nameof(SearchIndexDocument.Id), keys, cancellationToken);
    }


    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
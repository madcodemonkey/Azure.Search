using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using CustomBlobIndexer.Models;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer.Services;

/// <summary>
/// Processes a file and creates MULTIPLE search documents in the Azure Cognitive Search index based on the chunk size.
/// This chunking is done to limit the size of results we submit to Open AI.
/// </summary>
public class FileProcessChunkingService : IFileProcessService
{
    private readonly ServiceSettings _settings;
    private readonly ILogger<FileProcessService> _logger;
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ICustomComputerVisionService _computerVisionService;
    private readonly ICustomTextAnalyticsService _textAnalyticsService;
    private readonly ICustomSearchIndexService _searchIndexService;
    private readonly ITextChunkingService _textChunkingService;

    /// <summary>
    /// Constructor
    /// </summary>
    public FileProcessChunkingService(ServiceSettings settings,
        ILogger<FileProcessService> logger,
        IBlobSasBuilderService sasBuilderService,
        ICustomComputerVisionService computerVisionService,
        ICustomTextAnalyticsService textAnalyticsService,
        ICustomSearchIndexService searchIndexService,
        ITextChunkingService textChunkingService)
    {
        _settings = settings;
        _logger = logger;
        _sasBuilderService = sasBuilderService;
        _computerVisionService = computerVisionService;
        _textAnalyticsService = textAnalyticsService;
        _searchIndexService = searchIndexService;
        _textChunkingService = textChunkingService;
    }

    public async Task ProcessFileAsync(string name, Uri uri)
    {
        _logger.LogInformation($"Process file named: {name} located a uri: {uri} into chunks");

        string sasUrl = _sasBuilderService.GenerateSaSUrl(name, uri);

        _logger.LogInformation(sasUrl);

        string content = await _computerVisionService.ReadFileAsync(sasUrl);
        List<string> contentChunks = _textChunkingService.CreateChunks(content, _settings.ChunkMaximumNumberOfCharacters);
        var sourcePath = uri.GetPathAfterText(_settings.BlobContainerName);

        int chunkOrderNumber = 1;
        foreach (var contentChunk in contentChunks)
        {
            var id = Base64EncodeString($"{chunkOrderNumber}/{sourcePath}");

            var d = new SearchIndexDocument
            {
                Id = id,
                ChunkOrderNumber = chunkOrderNumber++, // allows you to put the chunks back together in the proper order.
                Content = contentChunk,
                SourcePath = uri.GetPathAfterText(_settings.BlobContainerName),
                Summary = await _textAnalyticsService.ExtractSummarySentenceAsync(contentChunk),
                Title = Path.GetFileName(name)
            };

            if (_settings.CognitiveSearchSkillDetectKeyPhrases)
            {
                d.KeyPhrases = await _textAnalyticsService.DetectedKeyPhrases(contentChunk);
            }

            if (_settings.CognitiveSearchSkillDetectLanguage)
            {
                d.Languages = await _textAnalyticsService.DetectLanguageInput(contentChunk);
            }

            if (_settings.CognitiveSearchSkillDetectEntities)
            {
                d.Entities = await _textAnalyticsService.DetectedEntitiesAsync(contentChunk);
            }

            if (_settings.CognitiveSearchSkillDetectSentiment)
            {
                //d.Sentiments = await _textAnalyticsService.DetectedSentiment(contentChunk);
            }

            if (_settings.CognitiveSearchSkillRedactText)
            {
                d.RedactedText = await _textAnalyticsService.RedactedText(contentChunk);
            }

            _searchIndexService.UploadDocuments(d);
        }

        await DeleteAnyRemainingChunksAsync(chunkOrderNumber, sourcePath);
    }

    /// <summary>
    /// If we are overwriting an existing document, it's possible that it has fewer chunks than the last time
    /// it was broken apart.  We should deleting those remaining chunks.
    /// </summary>
    /// <param name="chunkOrderNumber">The first chunk number that should be deleted.</param>
    /// <param name="sourcePath">The name and partial path to the file.  This should be unique if our data is coming out of ONE blob storage container.</param>
    /// <returns></returns>
    private async Task DeleteAnyRemainingChunksAsync(int chunkOrderNumber, string sourcePath)
    {
        var options = new SearchOptions
        {
            QueryType = SearchQueryType.Simple,
            IncludeTotalCount = true,
            Select = { nameof(SearchIndexDocument.Id) },
            Filter = $"{nameof(SearchIndexDocument.SourcePath)} eq '{sourcePath}' and {nameof(SearchIndexDocument.ChunkOrderNumber)} ge {chunkOrderNumber}"
        };
        
        var result = await _searchIndexService.SearchAsync<SearchIndexDocument>("*", options);

        List<string> keys = result.Docs.Select(s => s.Document.Id).ToList();
        
        await _searchIndexService.DeleteDocumentsAsync(nameof(SearchIndexDocument.Id), keys);
    }


    private string Base64EncodeString(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
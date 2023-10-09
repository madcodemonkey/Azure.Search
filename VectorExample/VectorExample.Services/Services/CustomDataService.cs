using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CogSimple.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VectorExample.Models;

namespace VectorExample.Services;

public class CustomDataService : ICustomDataService
{
    private readonly ApplicationSettings _appSettings;
    private readonly ICogSearchIndexService _indexService;
    private readonly IOpenAIService _openAIService;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomDataService(IOptions<ApplicationSettings> appSettings, ICogSearchIndexService indexService,
        IOpenAIService openAIService)
    {
        _appSettings = appSettings.Value;
        _indexService = indexService;
        _openAIService = openAIService;
    }


    /// <summary>
    /// Creates documents inside the index if none exist.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<bool> CreateDocumentsAsync(CancellationToken cancellationToken = default)
    {
        var docsInTheIndex = await _indexService.DocumentCountAsync(_appSettings.CognitiveSearchIndexName, cancellationToken);
        if (docsInTheIndex > 0)
        {
            return false;
        }

        List<SearchIndexDocument> docs = new List<SearchIndexDocument>();

        foreach (AzureCatalogData oneItem in GetData())
        {
            docs.Add(new SearchIndexDocument()
            {
                Id = oneItem.Id,
                Category = oneItem.Category,
                Title = oneItem.Title,
                Content = oneItem.Content,
                ContentVector = new List<float>(),
                TitleVector = new List<float>(),
                VectorEmbeddingVersion = 0
            });
            
        }

        await _indexService.UploadDocumentsAsync(_appSettings.CognitiveSearchIndexName, docs, cancellationToken);

        return docs.Any();
    }

    /// <summary>
    /// Updates the embeddings for vector fields on any document that has the incorrect Embedding version number.
    /// Documents that are created without embeddings have a version number of zero and will be updated by calling this method.
    /// </summary>
    /// <param name="batchSize">The number of items to update</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task<int> UpdateEmbeddingsAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        int numberUpdated = 0;
        var docsToUpdate = await FindDocsThatNeedEmbeddingAsync(batchSize, cancellationToken);
        var itemsUpdate = new List<SearchIndexDocument>();

        foreach (var doc in docsToUpdate.Docs)
        {
            doc.Document.TitleVector = await _openAIService.GenerateEmbeddingAsync(doc.Document.Title, cancellationToken);
            doc.Document.ContentVector = await _openAIService.GenerateEmbeddingAsync(doc.Document.Content, cancellationToken);
            doc.Document.VectorEmbeddingVersion = _openAIService.EmbeddingVersion;
            itemsUpdate.Add(doc.Document);

            if (itemsUpdate.Count > 9)
            {
                numberUpdated += await SaveDocumentsAsync(itemsUpdate, cancellationToken);
            }
        }

        numberUpdated += await SaveDocumentsAsync(itemsUpdate, cancellationToken);

        return numberUpdated;
    }

    /// <summary>
    /// Saves a batch of documents.
    /// </summary>
    /// <param name="itemsUpdate">The items to update and then clear</param>
    /// <param name="cancellationToken">The cancellation token</param>
    private async Task<int> SaveDocumentsAsync(List<SearchIndexDocument> itemsUpdate, CancellationToken cancellationToken)
    {
        if (itemsUpdate.Count == 0)
            return 0;

        await _indexService.UploadDocumentsAsync(_appSettings.CognitiveSearchIndexName, itemsUpdate, cancellationToken);
        
        var numberUpdated = itemsUpdate.Count;
        itemsUpdate.Clear();

        return numberUpdated;
    }

    /// <summary>
    /// Finds documents that don't have the correct embedding version number.
    /// </summary>
    /// <param name="batchSize">The batch size</param>
    /// <param name="cancellationToken">The cancellation token</param>
    private async Task<SearchQueryResponse<SearchIndexDocument>> FindDocsThatNeedEmbeddingAsync(int batchSize,  
        CancellationToken cancellationToken = default)
    {
        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            QueryType = SearchQueryType.Simple,
            SearchMode = SearchMode.Any,
            Filter = $"{nameof(SearchIndexDocument.VectorEmbeddingVersion)} ne {_openAIService.EmbeddingVersion}",
            Skip = 0,
            Size = batchSize
        };

        var result = await _indexService.SearchAsync<SearchIndexDocument>(_appSettings.CognitiveSearchIndexName, "*", options,
            cancellationToken);

        return result;
    }

    /// <summary>
    /// Gets some embedded data that is used to create the documents.
    /// </summary>
    private List<AzureCatalogData> GetData()
    {
        var stringData = DataFileLoader.GetFileDataAsString("AzureCatalog.json");
        var data = JsonConvert.DeserializeObject<List<AzureCatalogData>>(stringData);
        return data ?? new List<AzureCatalogData>();
    }
}
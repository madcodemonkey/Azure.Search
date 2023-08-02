using Search.CogServices;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

/// <summary>
/// Person Index Service inherits from AcmeCogIndexService because it will manipulate documents within the index.
/// If we want to search the index, there are three different services for that: <see cref="AcmeCogSearchService"/>,
/// <see cref="AcmeCogSuggestService"/> or <see cref="AcmeCogAutoCompleteService"/>
/// </summary>
public class PersonIndexService : AcmeCogIndexService, IPersonIndexService
{
    private readonly IndexerAppSettings _indexerAppSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public PersonIndexService(IndexerAppSettings indexerAppSettings,  AcmeCogSettings settings, 
        IAcmeCogClientService clientService,
        IAcmeCogSearchService cogSearchService) : base(settings, clientService, cogSearchService)
    {
        _indexerAppSettings = indexerAppSettings;
    }

    public async Task UpdateDocumentAsync(PersonIndexDocument personDoc)
    {
        await this.UploadDocumentAsync(_indexerAppSettings.CognitiveSearchIndexName, personDoc);
    }

    public async Task CreateDocumentAsync(PersonIndexDocument personDoc)
    {
        await this.UploadDocumentAsync(_indexerAppSettings.CognitiveSearchIndexName, personDoc);
    }

    public async Task DeleteDocumentAsync(string id)
    {
        await this.DeleteDocumentAsync(_indexerAppSettings.CognitiveSearchIndexName, nameof(PersonIndexDocument.Id), id);
    }
}
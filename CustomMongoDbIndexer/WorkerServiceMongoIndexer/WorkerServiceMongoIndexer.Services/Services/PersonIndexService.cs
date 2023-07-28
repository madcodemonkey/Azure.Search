using Azure.Search.Documents.Models;
using CogSearchServices.Services;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public class PersonIndexService : CogSearchIndexService, IPersonIndexService
{
    private readonly CogSearchServiceSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public PersonIndexService(CogSearchServiceSettings settings, 
        ICogSearchClientService clientService) : base(clientService)
    {
        _settings = settings;
    }
 
    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    public void UploadDocuments(PersonIndexDocument doc)
    {
        IndexDocumentsBatch<PersonIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        var searchClient = this.ClientService.GetSearchClient();
        IndexDocumentsResult result = searchClient.IndexDocuments(batch);
    }
 
}
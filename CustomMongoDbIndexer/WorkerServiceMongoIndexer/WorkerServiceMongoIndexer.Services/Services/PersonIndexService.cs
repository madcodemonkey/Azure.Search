using CogSearchServices.Services;
using WorkerServiceMongoIndexer.Models;

namespace WorkerServiceMongoIndexer.Services;

public class PersonIndexService : CogSearchIndexService, IPersonIndexService
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PersonIndexService(ICogSearchClientService clientService) : base(clientService)
    {
    }
 
    public async Task UpdateDocumentAsync(PersonIndexDocument personDoc)
    {
        await this.UploadDocumentAsync(personDoc);
    }

    public async Task CreateDocumentAsync(PersonIndexDocument personDoc)
    {
        await this.UploadDocumentAsync(personDoc);
    }

    public async Task DeleteDocumentAsync(string id)
    {
        await this.DeleteDocumentAsync(nameof(PersonIndexDocument.Id), id);
    }
}
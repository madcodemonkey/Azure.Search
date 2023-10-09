using Azure;

namespace CogSimple.Services;

public class CogSearchIndexerService : ICogSearchIndexerService
{
    protected ICogClientWrapperService ClientService { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CogSearchIndexerService(ICogClientWrapperService clientService)
    {
        ClientService = clientService;
    }
    
    /// <summary>
    /// Indicates if an indexer exists or not
    /// </summary>
    /// <param name="indexerName">The name of the indexer</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task<bool> IndexerExistsAsync(string indexerName, CancellationToken cancellationToken)
    {
        var indexerClient = ClientService.GetIndexerClient();

        Response<IReadOnlyList<string>>? indexerNamesAsync = await indexerClient.GetIndexerNamesAsync(cancellationToken);

        foreach (var item in indexerNamesAsync.Value)
        {
            if (string.IsNullOrWhiteSpace(item)) continue;
            if (indexerName == item) return true;
        }

        return false;
    }

    /// <summary>
    /// Deletes an indexer
    /// </summary>
    /// <param name="indexerName">The name of the indexer</param>
    /// <param name="checkIfExistsFirst">Indicates if you want the code to check to make sure the indexer exists before attempting to delete it.  If you try
    /// to delete an indexer that doesn't exist, it will generate an exception.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    public async Task DeleteIndexerAsync(string indexerName, bool checkIfExistsFirst, CancellationToken cancellationToken = default)
    {
        if (checkIfExistsFirst && await IndexerExistsAsync(indexerName, cancellationToken) == false)
        {
            return;
        }

        var indexerClient = ClientService.GetIndexerClient();
        await indexerClient.DeleteIndexerAsync(indexerName, cancellationToken);
    }
}
namespace VectorExample.Services;

public interface ICustomDataService
{
    /// <summary>
    /// Creates documents inside the index if none exist.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<bool> CreateDocumentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the embeddings for vector fields on any document that has the incorrect Embedding version number.
    /// Documents that are created without embeddings have a version number of zero and will be updated by calling this method.
    /// </summary>
    /// <param name="batchSize">The number of items to update</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    Task<int> UpdateEmbeddingsAsync(int batchSize, CancellationToken cancellationToken = default);
}
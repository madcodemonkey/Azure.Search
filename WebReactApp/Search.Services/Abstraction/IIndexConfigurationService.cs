using Search.Model;

namespace Search.Services;

public interface IIndexConfigurationService
{
    Task<List<IndexConfiguration>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IndexConfiguration> GetOrCreateAsync(string indexName,
        CancellationToken cancellationToken = default);

    Task SaveAsync(IndexConfiguration configuration, CancellationToken cancellationToken = default);
}
using Microsoft.Extensions.Caching.Memory;
using Search.Model;
using Search.Repositories;

namespace Search.Services;

public class IndexConfigurationService : IIndexConfigurationService
{
    private readonly TimeSpan _timeout = TimeSpan.FromHours(1);
    private readonly IMemoryCache _memoryCache;
    private readonly IIndexConfigurationRepository _configurationRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    public IndexConfigurationService(IMemoryCache memoryCache, IIndexConfigurationRepository configurationRepository)
    {
        _memoryCache = memoryCache;
        _configurationRepository = configurationRepository;
    }

    public async Task<List<IndexConfiguration>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _configurationRepository.GetAsync(cancellationToken);
        return result;
    }

    public async Task<IndexConfiguration> GetOrCreateAsync(string indexName,
        CancellationToken cancellationToken = default)
    {
        var configuration = _memoryCache.Get<IndexConfiguration>(indexName);
        if (configuration == null)
        {
            configuration = await _configurationRepository.GetFirstOrDefaultAsync(w => w.IndexName == indexName, cancellationToken);
            if (configuration == null)
            {
                configuration = CreateWithDefaults(indexName);
                await _configurationRepository.AddAsync(configuration, true, cancellationToken);
            }

            _memoryCache.Set(indexName, configuration, _timeout);
        }

        return configuration;
    }

    public async Task SaveAsync(IndexConfiguration configuration, CancellationToken cancellationToken = default)
    {
        var item = await _configurationRepository.GetFirstOrDefaultAsync(w => w.IndexName == configuration.IndexName, cancellationToken);
        if (item == null)
        {
            item = CreateWithDefaults(configuration.IndexName);
            await _configurationRepository.AddAsync(item, true, cancellationToken);
        }
        else
        {
            item.SecurityTrimmingField = configuration.SecurityTrimmingField;
            item.UsesCamelCaseFieldNames = configuration.UsesCamelCaseFieldNames;
            await _configurationRepository.UpdateAsync(item, true, cancellationToken);
        }

        _memoryCache.Set(configuration.IndexName, configuration, _timeout);
    }

    private IndexConfiguration CreateWithDefaults(string indexName)
    {
        return new IndexConfiguration
        {
            IndexName = indexName,
            SecurityTrimmingField = null,
            UsesCamelCaseFieldNames = false,
        };
    }
}
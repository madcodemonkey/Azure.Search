using Microsoft.Extensions.Caching.Memory;

namespace CustomSqlServerIndexer.Services;

public class HighWaterMarkStorageService : IHighWaterMarkStorageService
{
    private readonly IMemoryCache _memoryCache;  // Requires Microsoft.Extensions.Caching.Memory NuGt package
    private const string HighWatermarkKeyName = "HighWaterMark";

    public HighWaterMarkStorageService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<byte[]> GetHighWaterMarkRowVersionAsync()
    {
        var cachedData = _memoryCache.Get<byte[]>(HighWatermarkKeyName);
        if (cachedData == null)
        {
            return await Task.FromResult(Array.Empty<byte>());
        }

        return await Task.FromResult(cachedData);
    }

    public async Task<bool> SetHighWaterMarkRowVersionAsync(byte[] lastItem)
    {
        _memoryCache.Set(HighWatermarkKeyName, lastItem);
        
        // TODO: Store it.

        return await Task.FromResult(true);
    }
     
}
namespace CustomSqlServerIndexer.Services;

public interface IHighWaterMarkStorageService
{
    Task<byte[]> GetHighWaterMarkRowVersionAsync();
    Task<bool> SetHighWaterMarkRowVersionAsync(byte[] lastItem); 
}
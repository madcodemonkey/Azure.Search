namespace OutOfTheBoxBlobIndexer.Services.Services;

public interface IOutOfBoxIndexService
{
    /// <summary>
    /// Create the out-of-the-box Apache Gremlin indexER.
    /// </summary>
    Task CreateAsync();
}
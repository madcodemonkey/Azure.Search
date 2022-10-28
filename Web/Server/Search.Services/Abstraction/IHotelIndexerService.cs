using Search.CogServices;

namespace Search.Services;

public interface IHotelIndexerService : IAcmeSearchIndexerService
{
    /// <summary>Creates the Hotel indexer</summary>
    public Task<bool> CreateAsync();

    /// <summary>Deletes the hotel indexer</summary>
    Task<bool> DeleteAsync();

    /// <summary>Runs the hotel indexer now.</summary>
    Task RunAsync();

}
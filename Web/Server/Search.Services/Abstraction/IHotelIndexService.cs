namespace Search.Services;

public interface IHotelIndexService : ISearchIndexService
{
    /// <summary>Creates or updates the hotel index.</summary>
    Task<bool> CreateOrUpdateAsync();

    /// <summary>Deletes the hotel index.</summary>
    Task<bool> DeleteAsync();

}
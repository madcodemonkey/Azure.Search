namespace Search.Services;

public interface IHotelIndexService : IAcmeSearchIndexService
{
    /// <summary>Creates or updates the hotel index.</summary>
    Task<bool> CreateOrUpdateAsync();

    /// <summary>Deletes the hotel index.</summary>
    Task<bool> DeleteAsync();

}
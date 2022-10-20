namespace Search.Services;

public interface IHotelDataSourceService : IAcmeSearchDataSourceService
{
    /// <summary>Creates a hotel data source if it doesn't exist.</summary>
    Task<bool> CreateAsync();

    /// <summary>Deletes a hotel data source if it exists.</summary>
    Task<bool> DeleteAsync();
}
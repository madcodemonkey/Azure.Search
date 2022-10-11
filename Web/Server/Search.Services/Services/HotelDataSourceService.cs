namespace Search.Services;

public class HotelDataSourceService : SearchDataSourceService, IHotelDataSourceService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelDataSourceService(SearchServiceSettings settings, ISearchIndexerService indexerService) : base(indexerService)
    {
        _settings = settings;
    }
    
    /// <summary>Creates a hotel data source if it doesn't exist.</summary>
    public async Task<bool> CreateAsync()
    {
        return await this.CreateForAzureSqlAsync(
            _settings.DatabaseDataSourceName, 
            _settings.HotelTableName,
            _settings.DatabaseConnectionString);
    }

    /// <summary>Deletes a hotel data source if it exists.</summary>
    public async Task<bool> DeleteAsync()
    {
        return await this.DeleteAsync(_settings.DatabaseDataSourceName);
    }
}
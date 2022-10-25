using Search.Repositories;

namespace Search.Services;

public class HotelDataSourceService : AcmeSearchDataSourceService, IHotelDataSourceService
{
    private readonly SearchServiceSettings _settings;
    private readonly AcmeDatabaseOptions _databaseOptions;

    /// <summary>Constructor</summary>
    public HotelDataSourceService(SearchServiceSettings settings, AcmeDatabaseOptions databaseOptions, IAcmeSearchIndexerService indexerService) : base(indexerService)
    {
        _settings = settings;
        _databaseOptions = databaseOptions;
    }
    
    /// <summary>Creates a hotel data source if it doesn't exist.</summary>
    public async Task<bool> CreateAsync()
    {
        return await this.CreateForAzureSqlAsync(
            _settings.Hotel.DataSourceName, 
            _settings.Hotel.TableName,
            _databaseOptions.ConnectionString);
    }

    /// <summary>Deletes a hotel data source if it exists.</summary>
    public async Task<bool> DeleteAsync()
    {
        return await this.DeleteAsync(_settings.Hotel.DataSourceName);
    }
}
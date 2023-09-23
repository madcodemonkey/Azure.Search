using Search.CogServices;
using Search.Repositories;

namespace Search.Services;

public class HotelDataSourceService : AcmeSearchDataSourceService, IHotelDataSourceService
{
    private readonly AcmeDatabaseOptions _databaseOptions;
    private readonly SearchServiceSettings _settings;

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
            _databaseOptions.ConnectionString,
            _settings.Hotel.HighWaterMarkColumnName,
            _settings.Hotel.SoftDeleteColumnName,
            _settings.Hotel.SoftDeleteColumnValue);
    }

    /// <summary>Deletes a hotel data source if it exists.</summary>
    public async Task<bool> DeleteAsync()
    {
        return await this.DeleteAsync(_settings.Hotel.DataSourceName);
    }
}
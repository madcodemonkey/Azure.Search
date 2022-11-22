using Search.CogServices;

namespace Search.Services;

public class SearchServiceSettings : AcmeSearchSettings
{
    public string DatabaseConnectionString { get; set; }

    public string HotelSuggestorName { get; set; }
    public string HotelTableName { get; set; }
    public string HotelDataSourceName { get; set; }
    public string HotelIndexerName { get; set; }
    public string HotelSynonymMapName { get; set; }
    public string HotelIndexName { get; set; }
}
namespace Search.Services;

public class SearchServiceSettings
{
    public string SearchEndPoint { get; set; }
    public string SearchApiKey { get; set; }
    public string SearchSynonymMapName { get; set; }


    public string SearchAzureSqlConnectionString { get; set; }
    public string SearchAzureSqlTableName { get; set; }
    public string SearchAzureSqlDataSourceName { get; set; }
    public string SearchAzureSqlIndexerName { get; set; }
}


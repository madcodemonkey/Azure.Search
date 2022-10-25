namespace Search.Services;

public class SearchServiceIndexSettings
{
    public string DataSourceName { get; set; }

    public string SuggestorName { get; set; }
    public string IndexName { get; set; }
    public string TableName { get; set; }
    public string IndexerName { get; set; }
}
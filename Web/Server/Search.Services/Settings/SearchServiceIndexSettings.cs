namespace Search.Services;

public class SearchServiceIndexSettings
{
    public string DataSourceName { get; set; }

    /// <summary>The high watermark field used to detect changes</summary>
    public string HighWaterMarkColumnName { get; set; }

    public string IndexerName { get; set; }
    public string IndexName { get; set; }

    /// <summary>The column that indicates that the record should be removed from the Azure Search Index</summary>
    public string SoftDeleteColumnName { get; set; }

    /// <summary>The value in the column that indicates that the record should be deleted.</summary>
    public string SoftDeleteColumnValue { get; set; }

    public string SuggestorName { get; set; }
    public string TableName { get; set; }
}
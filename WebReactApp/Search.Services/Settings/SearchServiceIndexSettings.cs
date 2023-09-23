namespace Search.Services;

public class SearchServiceIndexSettings
{
    public string DataSourceName { get; set; } = string.Empty;

    /// <summary>The high watermark field used to detect changes</summary>
    public string HighWaterMarkColumnName { get; set; } = string.Empty;

    public string IndexerName { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;

    /// <summary>The column that indicates that the record should be removed from the Azure Search Index</summary>
    public string SoftDeleteColumnName { get; set; } = string.Empty;

    /// <summary>The value in the column that indicates that the record should be deleted.</summary>
    public string SoftDeleteColumnValue { get; set; } = string.Empty;

    public string SuggestorName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
}
namespace Search.Model;

public class IndexConfiguration
{
    public string IndexName { get; set; } = string.Empty;

    public string? SecurityTrimmingField { get; set; }
    public bool UsesCamelCaseFieldNames { get; set; }
}
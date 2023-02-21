namespace Search.CogServices;

public class AcmeSearchOrderBy
{
    /// <summary>The name of the Azure Search field to order by (it is case sensitive).</summary>
    public string FieldName { get; set; } = null!;

    /// <summary>If true, indicates we want to sort in descending order</summary>
    public bool SortDescending { get; set; }
}
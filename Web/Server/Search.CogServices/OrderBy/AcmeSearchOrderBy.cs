namespace Search.CogServices;

public class AcmeSearchOrderBy
{
    // TODO: Delete FieldId
    /// <summary>The id of the field to sort by.</summary>
    public int FieldId { get; set; }

    /// <summary>The name of the Azure Search field to order by (it is case sensitive).</summary>
    public int FieldName { get; set; }

    /// <summary>If true, indicates we want to sort in descending order</summary>
    public bool SortDescending { get; set; }
}
namespace Search.CogServices;

public class AcmeSearchOrderBy
{
    /// <summary>The id of the field to sort by.</summary>
    public int FieldId { get; set; }

    /// <summary>If true, indicates we want to sort in descending order</summary>
    public bool SortDescending { get; set; }
    
}
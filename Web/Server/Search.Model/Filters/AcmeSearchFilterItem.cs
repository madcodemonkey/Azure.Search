namespace Search.Model;

public class AcmeSearchFilterItem
{
    public int Id { get; set; }

    public AcmeSearchFilterOperatorEnum Operator { get; set; } = AcmeSearchFilterOperatorEnum.Equal;

    public string Value { get; set; }
}
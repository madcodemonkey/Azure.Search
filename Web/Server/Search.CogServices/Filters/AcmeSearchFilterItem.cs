namespace Search.CogServices;

public class AcmeSearchFilterItem
{
    public AcmeSearchFilterOperatorEnum Operator { get; set; } = AcmeSearchFilterOperatorEnum.Equal;

    public List<string?> Values { get; set; }
}
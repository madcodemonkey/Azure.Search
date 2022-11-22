namespace Search.CogServices;

public class AcmeSearchFilterItem
{
    public int Id { get; set; }

    public AcmeSearchFilterOperatorEnum Operator { get; set; } = AcmeSearchFilterOperatorEnum.Equal;

    public List<string> Values { get; set; }
}
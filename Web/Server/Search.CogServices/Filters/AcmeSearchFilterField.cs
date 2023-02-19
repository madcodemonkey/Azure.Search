namespace Search.CogServices;

/// <summary>This represents a single field and the one or more filters you want to have on that field.
/// This is typically used with facets where each facet is for a field but you can select more than one facet
/// and create more that one filters for that field.</summary>
/// <example>
/// Example 1:
/// (HotelType eq 'Luxury')
/// Example 2:
/// (HotelType eq 'Luxury' AND HotelType eq 'Budget')
/// Example 3:
/// (HotelType eq 'Luxury' OR HotelType eq 'Budget')
/// Example 3:
/// (HotelType ne 'Luxury' AND HotelType ne 'Budget')
/// </example>
public class AcmeSearchFilterField
{
    /// <summary>The id of the field.</summary>
    public string FieldName { get; set; }

    public AcmeSearchFilterFieldTypeEnum FieldType {get; set;}

    /// <summary>The filters that will be used with this group.</summary>
    public List<AcmeSearchFilterItem> Filters { get; set; } = new();

    /// <summary>Indicates which logical operator to use with ALL its own Filters list.</summary>
    public AcmeSearchGroupOperatorEnum FiltersOperator { get; set; } = AcmeSearchGroupOperatorEnum.And;

    /// <summary>Indicates which logical operator to use with its NEXT peer.</summary>
    public AcmeSearchGroupOperatorEnum PeerOperator { get; set; } = AcmeSearchGroupOperatorEnum.And;
}
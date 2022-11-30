namespace Search.CogServices;

/// <summary>This represents a grouping of filters on a page.  If all the filters on the page are AND'ed together, you need only one of these;
/// however, if you have a scenario where a group needs to be OR'ed and then AND'ed, you will need one for every group on the page.</summary>
/// <example>
/// Case for only one group
/// (HotelType = 'Luxury' AND City = 'Seattle')
/// Note 1: Here everything is AND together
/// Note 2: The user can check the Facet boxes and get an immediate search
/// 
/// Case for two groups
/// (HotelType = 'Luxury' OR HotelType = 'Normal') AND (City = 'Seattle' OR City = 'SeaTac')
/// Note 1: The parenthesis represent two instances of AcmeSearchFilterGroup
/// Note 2: The two HotelType filters would go in an group by themselves so that they can be OR'ed together
/// Note 3: The two City filters would go in an group by themselves so that they can be OR'ed together 
/// Note 4: You could not do an immediate search if the user can check a Facet box because they can select multiples within a facet or at the very least you could not refresh the facet list 
/// </example>
public class AcmeSearchFilterGroup
{
    /// <summary>Indicates which logical operator to use with its peers.  If true, use the AND operator; otherwise, use the OR operator.</summary>
    public AcmeSearchGroupOperatorEnum PeerOperator { get; set; } = AcmeSearchGroupOperatorEnum.And;

    /// <summary>Indicates which logical operator to use with its own Filters list.  If true, use the AND operator; otherwise, use the OR operator.</summary>
    public AcmeSearchGroupOperatorEnum FiltersOperator { get; set; } = AcmeSearchGroupOperatorEnum.And;

    /// <summary>The filters that will be used with this group.</summary>
    public List<AcmeSearchFilterItem> Filters { get; set; } = new();
}
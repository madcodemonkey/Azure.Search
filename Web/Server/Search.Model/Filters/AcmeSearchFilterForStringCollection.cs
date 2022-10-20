using System.Text;

namespace Search.Model;

public class AcmeSearchFilterForStringCollection : AcmeSearchFilterBase
{
    public AcmeSearchFilterForStringCollection(int id, string displayName, string fieldName, bool isFacetable, bool isSecurityFilter)
        : base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    {
    }
    /// <summary>Get Filter</summary>
    /// <param name="searchOperator">Currently only handles equal.  There are other options</param>
    /// <param name="values"></param>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, params string[] values)
    {
        if (values == null || values.Length == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal)
            throw new ArgumentException($"Currently we only handle equal operator for collections!  Please correct the search operator for the field named {FieldName}");

        // Do not make sb a member of the class. 
        //  Create it each time in case we make this class a singleton!
        var sb = new StringBuilder();

        // Using shorthand syntax for equal reference: https://docs.microsoft.com/bs-latn-ba/azure/search/search-query-odata-search-in-function
        // Another example: https://docs.microsoft.com/en-us/azure/search/search-security-trimming-for-azure-search-with-aad#step-2-compose-the-search-request
        // Note: The each item is NOT surrounded by single quotes.  ALL items are surround by a pair of single quotes.
        //       You need OR syntax to do that..see shorthand syntax link above and look at the top of the page.
        sb.Append($"{this.FieldName}/any(g:search.in(g, '");

        // Length of string is greater than zero now, so need a way of tracking when a comma is needed.
        bool commaNeeded = false;
        foreach (string item in values)
        {
            if (commaNeeded)
                sb.Append(",");
            sb.Append($"{item}");
            commaNeeded = true;
        }

        sb.Append("', ','))");  // Set delimiter as comma delimited.

        return sb.ToString();
    }
}
using System.Text;

namespace Search.CogServices;

public class AcmeSearchODataHandlerStringCollection : AcmeSearchODataHandlerBase
{
    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public override bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType) => fieldType == AcmeSearchFilterFieldTypeEnum.StringCollection;

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal)
            throw new ArgumentException($"Currently we only handle equal operator for collections!  Please correct the search operator for the field named {fieldName}");

        var valueList = RemoveNulls(values);
        if (valueList.Count == 0)
        {
            return string.Empty;
        }

        // Do not make sb a member of the class.
        //  Create it each time in case we make this class a singleton!
        var sb = new StringBuilder();

        // Using shorthand syntax for equal reference: https://docs.microsoft.com/bs-latn-ba/azure/search/search-query-odata-search-in-function
        // Another example: https://docs.microsoft.com/en-us/azure/search/search-security-trimming-for-azure-search-with-aad#step-2-compose-the-search-request
        // Note: The each item is NOT surrounded by single quotes.  ALL items are surround by a pair of single quotes.
        //       You need OR syntax to do that..see shorthand syntax link above and look at the top of the page.
        sb.Append($"{fieldName}/any(g:search.in(g, '");

        // Length of string is greater than zero now, so need a way of tracking when a comma is needed.
        bool commaNeeded = false;
        foreach (string? item in valueList)
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
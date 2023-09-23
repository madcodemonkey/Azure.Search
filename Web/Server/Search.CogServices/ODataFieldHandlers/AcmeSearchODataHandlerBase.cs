using System.Text.RegularExpressions;

namespace Search.CogServices;

public abstract class AcmeSearchODataHandlerBase : IAcmeSearchODataHandler
{
    /// <summary>Creates an OData filter string.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    public string CreateFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            throw new ArgumentException("You must specify one or more values for a filter!");
        
        var scrubbedFieldValues = ScrubFieldValues(values);
        var scrubbedFieldName = Regex.Replace(fieldName, @"(\(|\)|')", "");
        return GetFilter(scrubbedFieldName, searchOperator, scrubbedFieldValues);
    }

    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public abstract bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType);

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected abstract string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values);

    protected string OperatorToString(AcmeSearchFilterOperatorEnum searchOperator)
    {
        // Reference https://docs.microsoft.com/bs-latn-ba/azure/search/search-query-odata-comparison-operators
        switch (searchOperator)
        {
            case AcmeSearchFilterOperatorEnum.Equal:
                return "eq";

            case AcmeSearchFilterOperatorEnum.NotEqual:
                return "ne";

            case AcmeSearchFilterOperatorEnum.GreaterThan:
                return "gt";

            case AcmeSearchFilterOperatorEnum.LessThan:
                return "lt";

            case AcmeSearchFilterOperatorEnum.GreaterOrEqual:
                return "ge";

            case AcmeSearchFilterOperatorEnum.LessOrEqual:
                return "le";

            case AcmeSearchFilterOperatorEnum.WithinRange:
                throw new ArgumentException(
                    "There is no conversion from 'WithinRange' to an OData operator!  It must be handled by each filter field class and not by the base class!");

            default:
                return "eq";
        }
    }

    /// <summary>Scrubs field values as necessary so that the user cannot circumvent security trimming.</summary>
    /// <param name="values"></param>
    private List<string?> ScrubFieldValues(List<string?> values)
    {
        var result = new List<string?>(values.Count);

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.Add(value);
                continue;
            }

            var valueToAdd = Regex.Replace(value, @"(\(|\))", "");
            
            if (valueToAdd.IndexOf("'", StringComparison.Ordinal) != -1)
               valueToAdd = valueToAdd.Replace("'", "''");
            
            result.Add(valueToAdd);
        }

        return result;
    }

    /// <summary>
    /// Removes all the nulls from the string list.
    /// </summary>
    /// <param name="values">A string list with possible nulls</param>
    /// <returns>A string list that cannot have nulls.</returns>
    protected List<string> RemoveNulls(List<string?> values)
    {
        var result = new List<string>();
        values.ForEach(i =>
        {
            if (i != null)
                result.Add(i);
        });

        return result;
    }
}
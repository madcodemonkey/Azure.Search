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
        return GetFilter(fieldName, searchOperator, scrubbedFieldValues);
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

            default:
                return "eq";
        }
    }

    /// <summary>Scrubs field values as necessary so that the user cannot circumvent security trimming.</summary>
    /// <param name="values"></param>
    private List<string?> ScrubFieldValues(List<string?> values)
    {
        var result = new List<string>(values.Count);

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.Add(value);
            }
            else if (value.IndexOf("'", StringComparison.Ordinal) != -1)
            {
                result.Add(value.Replace("'", "''"));
            }
            else
            {
                result.Add(value);
            }
        }

        return result;
    }
}
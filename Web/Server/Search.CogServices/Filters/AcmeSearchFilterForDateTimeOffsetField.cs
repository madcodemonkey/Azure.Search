namespace Search.CogServices;

public class AcmeSearchFilterForDateTimeOffsetField : AcmeSearchFilterBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFilterForDateTimeOffsetField(int id, string fieldName, string displayName, bool isFacetable, bool isSecurityFilter) :
        base(id, displayName, fieldName, isFacetable, isSecurityFilter)
    { }

    /// <summary>This protected method builds the filter for the DateTimeOffset type.</summary>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {
            if (values.Count < 2)
                throw new ArgumentException($"To use the {AcmeSearchFilterOperatorEnum.WithinRange} operator with a date time field, you must include at least two values!");

            return $"{this.FieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertDateStringToUtcString(values[0])} and " +
                $"{this.FieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertDateStringToUtcString(values[1])}";
        }

        return $"{this.FieldName} {OperatorToString(searchOperator)} {ConvertDateStringToUtcString(values[0])}";
    }

    private string ConvertDateStringToUtcString(string? dateString)
    {
        if (dateString == null) return "null";

        if (DateTime.TryParse(dateString, out var theDate))
        {
            var utcDate = new DateTime(theDate.Year, theDate.Month, theDate.Day, 0, 0, 0, DateTimeKind.Utc);
            return utcDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }


        throw new ArgumentException($"Unable to parse the date string '{dateString}' for the {this.FieldName} field as date time!");
    }
}
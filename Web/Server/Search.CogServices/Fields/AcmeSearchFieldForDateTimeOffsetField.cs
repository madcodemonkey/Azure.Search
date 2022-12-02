namespace Search.CogServices;

public class AcmeSearchFieldForDateTimeOffsetField : AcmeSearchFieldBase
{
    /// <summary>Constructor</summary>
    public AcmeSearchFieldForDateTimeOffsetField(int id, string propertyFieldName, string indexFieldName, string displayName,
        bool isFilterable, bool isSortable, bool isFacetable, bool isHighlighted, bool isSecurityFilter) : 
        base(id, propertyFieldName, indexFieldName, displayName, isFilterable, isSortable, isFacetable, isHighlighted, isSecurityFilter)
    {
    }


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

            return $"{this.IndexFieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertDateStringToUtcString(values[0], TimeHandlingEnum.UseMidnight)} and " +
                $"{this.IndexFieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertDateStringToUtcString(values[1], TimeHandlingEnum.UseEndOfDay)}";
        }

        return $"{this.IndexFieldName} {OperatorToString(searchOperator)} {ConvertDateStringToUtcString(values[0], TimeHandlingEnum.UseTimeAsSent)}";
    }

    private string ConvertDateStringToUtcString(string? dateString, TimeHandlingEnum timeHandling)
    {
        if (dateString == null) return "null";

        if (DateTime.TryParse(dateString, out var theDate))
        {
            DateTime utcDate;
            
            switch (timeHandling)
            {
                case TimeHandlingEnum.UseTimeAsSent:
                    utcDate = new DateTime(theDate.Year, theDate.Month, theDate.Day, theDate.Hour, theDate.Minute, theDate.Second, DateTimeKind.Utc);
                    break;
                case TimeHandlingEnum.UseMidnight:
                    utcDate = new DateTime(theDate.Year, theDate.Month, theDate.Day, 0, 0, 0, DateTimeKind.Utc);
                    break;
                case TimeHandlingEnum.UseEndOfDay:
                    utcDate = new DateTime(theDate.Year, theDate.Month, theDate.Day, 23, 59, 59, DateTimeKind.Utc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeHandling), timeHandling, null);
            }

            var utcDateAndTime = utcDate.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

            return utcDateAndTime;
        }


        throw new ArgumentException($"Unable to parse the date string '{dateString}' for the {this.IndexFieldName} field as date time!");
    }

    private enum TimeHandlingEnum
    {
        UseTimeAsSent,
        UseMidnight,
        UseEndOfDay
    }
}
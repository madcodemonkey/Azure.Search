namespace Search.CogServices;

public class AcmeSearchODataHandlerDateTimeOffset : AcmeSearchODataHandlerBase
{
    private enum TimeHandlingEnum
    {
        UseTimeAsSent,
        UseMidnight,
        UseEndOfDay
    }

    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public override bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType) => fieldType == AcmeSearchFilterFieldTypeEnum.DateTimeOffset;

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator == AcmeSearchFilterOperatorEnum.WithinRange)
        {
            if (values.Count < 2)
                throw new ArgumentException($"To use the {AcmeSearchFilterOperatorEnum.WithinRange} operator with a date time field, you must include at least two values!");

            return $"{fieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.GreaterOrEqual)} {ConvertDateStringToUtcString(fieldName, values[0], TimeHandlingEnum.UseMidnight)} and " +
                $"{fieldName} {OperatorToString(AcmeSearchFilterOperatorEnum.LessOrEqual)} {ConvertDateStringToUtcString(fieldName, values[1], TimeHandlingEnum.UseEndOfDay)}";
        }

        return $"{fieldName} {OperatorToString(searchOperator)} {ConvertDateStringToUtcString(fieldName, values[0], TimeHandlingEnum.UseTimeAsSent)}";
    }

    private string ConvertDateStringToUtcString(string fieldName, string? dateString, TimeHandlingEnum timeHandling)
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

        throw new ArgumentException($"Unable to parse the date string '{dateString}' for the {fieldName} field as date time!");
    }
}
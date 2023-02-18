namespace Search.CogServices;

public class AcmeSearchODataFieldChildObjectString : AcmeSearchODataFieldBase
{
    /// <summary>
    /// Indicates if the handler can projects the field type.
    /// </summary>
    /// <param name="fieldType">The type of field.</param>
    public override bool CanHandle(AcmeSearchFilterFieldTypeEnum fieldType) => fieldType == AcmeSearchFilterFieldTypeEnum.ChildObjectString;

    /// <summary>The method that is overriden by the superclass that will be specific to the type specified by the class name.</summary>
    /// <param name="fieldName">The name of the field in the Azure Index document (it is case sensitive!)</param>
    /// <param name="searchOperator">The operator to use while building the filter.</param>
    /// <param name="values">The values to use while building the filter.</param>
    /// <returns>An OData filer</returns>
    protected override string GetFilter(string fieldName, AcmeSearchFilterOperatorEnum searchOperator, List<string?> values)
    {
        string[] fields = fieldName.Split('/');
        if (fields == null || fields.Length != 2)
            throw new ArgumentException("Please use slash syntax to denote parent and child fields (e.g., 'authors/name' where authors is parent and name is child object field name)");
        string parentFieldName = fields[0];
        string childFieldName = fields[1];

        if (values == null || values.Count == 0)
            return string.Empty;

        if (searchOperator != AcmeSearchFilterOperatorEnum.Equal)
            throw new ArgumentException($"Currently we only handle equal operator for collections!  Please correct the search operator for the field named {fieldName}");

        //  return $"authors/any(c: c/name eq '{values[0]}')";
        return $"{parentFieldName}/any(c: c/{childFieldName} eq '{values[0]}')";
    }
}
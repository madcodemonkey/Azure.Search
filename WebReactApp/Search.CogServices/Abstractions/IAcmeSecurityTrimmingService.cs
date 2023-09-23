namespace Search.CogServices;

public interface IAcmeSecurityTrimmingService
{
    /// <summary>
    /// Initializes the field and required values.
    /// </summary>
    /// <param name="fieldName">
    /// The Cognitive Search index field name, which should be of type Collection(Edm.String).
    /// </param>
    /// <param name="fieldType">The string field type (i.e., string, number, boolean, datetime, stringcollection, childstring)</param>
    /// <param name="valuesThatMustExist">The values that must exist for the a document to be returned.</param>
    IAcmeSecurityTrimmingFilter? CreateFilter(string? fieldName, AcmeSearchFilterFieldTypeEnum fieldType, List<string?> valuesThatMustExist);
}
namespace Search.CogServices;

public class AcmeSecurityTrimmingService : IAcmeSecurityTrimmingService
{
    private readonly IAcmeODataService _searchODataBuilderService;

    /// <summary>
    /// Constructor
    /// </summary>
    public AcmeSecurityTrimmingService(IAcmeODataService searchODataBuilderService)
    {
        _searchODataBuilderService = searchODataBuilderService;
    }

    /// <summary>
    /// Initializes the field and required values.
    /// </summary>
    /// <param name="fieldName">
    /// The Cognitive Search index field name, which should be of type Collection(Edm.String).
    /// </param>
    /// <param name="fieldType">The field type</param>
    /// <param name="valuesThatMustExist">The values that must exist for the a document to be returned.</param>
    public IAcmeSecurityTrimmingFilter? CreateFilter(string? fieldName, AcmeSearchFilterFieldTypeEnum fieldType, List<string?> valuesThatMustExist)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
        {
            return null;
        }

        IAcmeSearchODataHandler? fieldHandler = _searchODataBuilderService.FindHandler(fieldType);
        if (fieldHandler == null)
        {
            return null;
        }

        return new AcmeSecurityTrimmingFilter(fieldName, valuesThatMustExist, fieldHandler);
    }
}
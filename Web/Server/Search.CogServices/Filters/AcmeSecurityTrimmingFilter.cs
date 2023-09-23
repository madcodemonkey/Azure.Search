using System.Text;

namespace Search.CogServices;

/// <summary>
/// Adds a security trimming filter when constructing an OData filter.  It is used for security trimming when the
/// security field in Cognitive Search is of type Collection(Edm.String)
/// </summary>
public class AcmeSecurityTrimmingFilter : IAcmeSecurityTrimmingFilter
{
    private readonly string? _fieldName;
    private readonly List<string?> _valuesThatMustExist;
    private readonly IAcmeSearchODataHandler _fieldHandler;

    /// <summary>
    /// Constructor
    /// </summary>
    public AcmeSecurityTrimmingFilter(string fieldName, List<string?> valuesThatMustExist, IAcmeSearchODataHandler fieldHandler)
    {
        _fieldName = fieldName;
        _valuesThatMustExist = valuesThatMustExist;
        _fieldHandler = fieldHandler;
    }

    /// <summary>
    /// The security tracking field name.
    /// </summary>
    public string? FieldName => _fieldName;

    /// <summary>
    /// Adds a filter to the provided string builder if necessary.
    /// </summary>
    /// <param name="sbFilter">The current filters in OData format</param>
    /// <param name="fieldFilterList">
    /// The field filter list that is being turned into an OData filter.
    /// </param>
    /// <remarks>
    /// If the field name you specified in the Initialize method does not exist OR is not
    /// filterable, this call will either do nothing if the field does not exist OR generate an
    /// exception when we call Cognitive Search because it's not filterable.
    /// </remarks>
    public void AddFilter(StringBuilder sbFilter, List<AcmeSearchFilterField> fieldFilterList)
    {
        if (string.IsNullOrWhiteSpace(_fieldName) ||
            _valuesThatMustExist.Count == 0)
        {
            return;
        }

        // Is there an existing filter?
        if (sbFilter.Length > 0)
        {
            // Did the user/client specify any filters? If so, surround them with parenthesis so
            // they cannot negate our security trimming.
            if (fieldFilterList.Count > 0)
                sbFilter.SurroundWithParenthesis();

            sbFilter.Append(" and ");
        }

        var searchInFilter = _fieldHandler.CreateFilter(_fieldName, AcmeSearchFilterOperatorEnum.Equal, _valuesThatMustExist);

        sbFilter.Append(searchInFilter);
    }
}
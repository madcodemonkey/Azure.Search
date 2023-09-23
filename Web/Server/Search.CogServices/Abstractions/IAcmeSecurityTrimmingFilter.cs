using System.Text;

namespace Search.CogServices;

public interface IAcmeSecurityTrimmingFilter
{
    /// <summary>
    /// The security tracking field name.
    /// </summary>
    public string? FieldName { get; }

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
    void AddFilter(StringBuilder sbFilter, List<AcmeSearchFilterField> fieldFilterList);
}
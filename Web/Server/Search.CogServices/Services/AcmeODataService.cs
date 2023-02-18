using System.Text;

namespace Search.CogServices.Services;

public interface IAcmeIndexInfoService
{
    string GetSecurityTrimmingFieldName(string indexName);

    bool UseSecurityTrimming(string indexName);
}

// TODO: GEt this from appsettings for now.
public class AcmeIndexInfoService : IAcmeIndexInfoService
{
    public string GetSecurityTrimmingFieldName(string indexName)
    {
        return "role";
    }

    public bool UseSecurityTrimming(string indexName)
    {
        return true;
    }
}

public class AcmeODataService
{
    private readonly IAcmeIndexInfoService _indexInfoService;
    private readonly List<IAcmeSearchODataHandler> _oDataFieldHandlers;

    /// <summary>Constructor</summary>
    public AcmeODataService(IAcmeIndexInfoService indexInfoService)
    {
        _indexInfoService = indexInfoService;
        _oDataFieldHandlers = new List<IAcmeSearchODataHandler>()
        {
            new AcmeSearchODataHandlerString(),
            new AcmeSearchODataHandlerNumber(),
            new AcmeSearchODataHandlerBoolean(),
            new AcmeSearchODataHandlerStringCollection(),
            new AcmeSearchODataHandlerDateTimeOffset(),
            new AcmeSearchODataHandlerChildObjectString(),
        };
    }

    /// <summary>Builds and OData filter for Azure Search based on user specified filters and the roles that user has been assigned.</summary>
    /// <param name="indexName">The name of the Azure Index</param>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    /// <param name="rolesTheUserIsAssigned">Roles assigned to the current user.</param>
    /// <returns>An OData Filter</returns>
    public string BuildODataFilter(string indexName, List<AcmeSearchFilterFieldV2> fieldFilters, List<string?> rolesTheUserIsAssigned)
    {
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE
        // All Filters are case SENSITIVE

        var sbFilter = new StringBuilder();

        for (var index = 0; index < fieldFilters.Count; index++)
        {
            var oneFieldFilter = fieldFilters[index];
            if (index > 0)
            {
                if (fieldFilters[index - 1].PeerOperator == AcmeSearchGroupOperatorEnum.And)
                    sbFilter.Append(" and ");
                else sbFilter.Append(" or ");
            }

            // Note: The call to BuildODataFilterForOneFieldFilter will surround the resulting OData filter
            //       with parenthesis if two or more filters are OR'ed together.
            string oneFieldODataFilter = BuildODataFilterForOneFieldFilter(oneFieldFilter);

            sbFilter.Append(oneFieldODataFilter);
        }

        // Warning!! If the object of T that you're passing into the Azure Suggest or Azure Search methods does not have the Roles property on it,
        //           using roles here will do NOTHING!!!  In other words, I didn't want to return roles to the user
        //           so I removed it from by BookDocumentBrief class. Afterwards, this filter STOPPED working!  No ERRORS!
        if (_indexInfoService.UseSecurityTrimming(indexName) && rolesTheUserIsAssigned.Count > 0)
        {
            string securityTrimmingFieldName = _indexInfoService.GetSecurityTrimmingFieldName(indexName);
            if (string.IsNullOrWhiteSpace(securityTrimmingFieldName))
            {
                if (sbFilter.Length > 0)
                {
                    if (ShouldSurroundAllTheFieldFiltersWithParenthesis(fieldFilters))
                        sbFilter.SurroundWithParenthesis();

                    sbFilter.Append(" and ");
                }

                IAcmeSearchODataHandler? fieldHandler = _oDataFieldHandlers.FirstOrDefault(w => w.CanHandle(AcmeSearchFilterFieldTypeEnum.StringCollection));

                if (fieldHandler == null)
                    throw new ArgumentNullException($"Could not find a OData field handler for a field named '{securityTrimmingFieldName}' with field type '{AcmeSearchFilterFieldTypeEnum.StringCollection}'");

                sbFilter.Append(fieldHandler.CreateFilter(securityTrimmingFieldName, AcmeSearchFilterOperatorEnum.Equal, rolesTheUserIsAssigned));
            }
        }

        return sbFilter.ToString();
    }

    /// <summary>Creates an OData filter for one group.</summary>
    /// <param name="fieldFilter">A grouping of filters for one field.</param>
    private string BuildODataFilterForOneFieldFilter(AcmeSearchFilterFieldV2 fieldFilter)
    {
        var sbGroupFilter = new StringBuilder();

        IAcmeSearchODataHandler? fieldHandler = _oDataFieldHandlers.FirstOrDefault(w => w.CanHandle(fieldFilter.FieldType));

        if (fieldHandler == null)
            throw new ArgumentNullException($"Could not find a OData field handler for a field named '{fieldFilter.FieldName}' with field type '{fieldFilter.FieldType}'");

        foreach (var filter in fieldFilter.Filters)
        {
            if (sbGroupFilter.Length > 0)
            {
                if (fieldFilter.FiltersOperator == AcmeSearchGroupOperatorEnum.And)
                    sbGroupFilter.Append(" and ");
                else sbGroupFilter.Append(" or ");
            }

            sbGroupFilter.Append(fieldHandler.CreateFilter(fieldFilter.FieldName, filter.Operator, filter.Values));
        }

        if (fieldFilter.Filters.Count > 1 && fieldFilter.FiltersOperator == AcmeSearchGroupOperatorEnum.Or)
            sbGroupFilter.SurroundWithParenthesis();

        return sbGroupFilter.ToString();
    }

    /// <summary>Determines if we should surround all the field list filers with parenthesis before adding security trimming</summary>
    /// <param name="fieldFilters">A list of field filter where each represents a grouping of filters for one field.</param>
    private bool ShouldSurroundAllTheFieldFiltersWithParenthesis(List<AcmeSearchFilterFieldV2> fieldFilters)
    {
        if (fieldFilters.Count == 0)
            return false;

        if (fieldFilters.Count == 1)
        {
            if (fieldFilters[0].Filters.Count == 1)
                return false;

            return fieldFilters[0].PeerOperator == AcmeSearchGroupOperatorEnum.Or;
        }

        foreach (var group in fieldFilters)
        {
            if (group.PeerOperator != AcmeSearchGroupOperatorEnum.And)
            {
                return true;
            }
        }

        return false;
    }
}
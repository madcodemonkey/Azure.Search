using Azure.Search.Documents.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Search.CogServices;
using Search.Services;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : SearchControllerBase
{
    private readonly IAcmeAutoCompleteService _autoCompleteService;
    private readonly IIndexConfigurationService _indexConfigurationService;
    private readonly IValidator<AcmeAutoCompleteQuery> _autoCompleteValidator;
    private readonly IAcmeSearchService _searchService;
    private readonly IValidator<AcmeSearchQuery> _searchValidator;
    private readonly IAcmeSuggestService _suggestService;
    private readonly IValidator<AcmeSuggestQuery> _suggestValidator;

    /// <summary>Constructor</summary>
    public SearchController(
        IValidator<AcmeSearchQuery> searchValidator,
        IValidator<AcmeSuggestQuery> suggestValidator,
        IValidator<AcmeAutoCompleteQuery> autoCompleteValidator,
        IAcmeSearchService searchService,
        IAcmeSuggestService suggestService,
        IAcmeAutoCompleteService autoCompleteService,
        IIndexConfigurationService indexConfigurationService)
    {
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
        _autoCompleteValidator = autoCompleteValidator;
        _searchService = searchService;
        _suggestService = suggestService;
        _autoCompleteService = autoCompleteService;
        _indexConfigurationService = indexConfigurationService;
    }

    [HttpPost("AutoComplete")]
    public async Task<IActionResult> AutoComplete(AcmeAutoCompleteQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _autoCompleteValidator.ValidateAsync(query, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }
        

        var result = await _autoCompleteService.AutoCompleteAsync(query, 
            await GetSecurityTrimmingFieldAsync(query.IndexName, cancellationToken));

        return Ok(result);
    }
    
    [HttpPost("Documents")]
    public async Task<IActionResult> Search(AcmeSearchQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _searchValidator.ValidateAsync(query, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var securityTrimmingFilter = await GetSecurityTrimmingFieldAsync(query.IndexName, cancellationToken);

        AcmeSearchQueryResult<SearchResult<SearchDocument>> searchResult = query.QueryType == SearchQueryType.Semantic ?
            await _searchService.SemanticSearchAsync(query, query.SemanticConfigurationName ?? string.Empty, securityTrimmingFilter) :
            await _searchService.SearchAsync(query, securityTrimmingFilter);

        return new OkObjectResult(searchResult);
    }

    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestQuery query, CancellationToken cancellationToken)
    {
        var validationResult = await _suggestValidator.ValidateAsync(query, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        SuggestResults<SearchDocument> suggestResult = await _suggestService.SuggestAsync(query,
            await GetSecurityTrimmingFieldAsync(query.IndexName, cancellationToken));

        return new OkObjectResult(suggestResult);
    }

    private async Task<IAcmeSecurityTrimmingFilter?> GetSecurityTrimmingFieldAsync(string indexName, CancellationToken cancellationToken)
    {
        var indexConfig = await _indexConfigurationService.GetOrCreateAsync(indexName, cancellationToken);
        if (string.IsNullOrWhiteSpace(indexConfig.SecurityTrimmingField))
            return null;

        return new AcmeSecurityTrimmingFilter(indexConfig.SecurityTrimmingField, GetRoles(), new AcmeSearchODataHandlerStringCollection());
    }
}
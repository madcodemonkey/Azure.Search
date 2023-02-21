using AutoMapper;
using Azure.Search.Documents.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Search.CogServices;
using Search.Web.Models;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : SearchControllerBase
{
    private readonly IAcmeAutoCompleteService _autoCompleteService;
    private readonly IValidator<AcmeAutoCompleteQuery> _autoCompleteValidator;
    private readonly IAcmeSearchService _searchService;
    private readonly IValidator<AcmeSearchQuery> _searchValidator;
    private readonly IAcmeSuggestService _suggestService;
    private readonly IValidator<AcmeSuggestQuery> _suggestValidator;

    /// <summary>Constructor</summary>
    public SearchController(
        IMapper mapper,
        IValidator<AcmeSearchQuery> searchValidator,
        IValidator<AcmeSuggestQuery> suggestValidator,
        IValidator<AcmeAutoCompleteQuery> autoCompleteValidator,
        IAcmeSearchService searchService,
        IAcmeSuggestService suggestService,
        IAcmeAutoCompleteService autoCompleteService)
    {
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
        _autoCompleteValidator = autoCompleteValidator;
        _searchService = searchService;
        _suggestService = suggestService;
        _autoCompleteService = autoCompleteService;
    }

    [HttpPost("AutoComplete")]
    public async Task<IActionResult> AutoComplete(AcmeAutoCompleteQuery query)
    {
        var validationResult = await _autoCompleteValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var result = await _autoCompleteService.AutoCompleteAsync(query, "roles", GetRoles());

        return Ok(result);
    }

    [HttpPost("Documents")]
    public async Task<IActionResult> Search(AcmeSearchQuery query)
    {
        var validationResult = await _searchValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        AcmeSearchQueryResult<SearchResult<SearchDocument>> searchResult = query.QueryType == SearchQueryType.Semantic ?
            await _searchService.SemanticSearchAsync(query, query.SemanticConfigurationName ?? string.Empty, "roles", GetRoles()) :
            await _searchService.SearchAsync(query, "roles", GetRoles());

        return new OkObjectResult(searchResult);
    }

    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestQuery query)
    {
        var validationResult = await _suggestValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        SuggestResults<SearchDocument> suggestResult = await _suggestService.SuggestAsync(query, "roles", GetRoles());

        return new OkObjectResult(suggestResult);
    }
}
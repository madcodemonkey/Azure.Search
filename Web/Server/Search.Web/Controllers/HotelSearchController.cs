using Azure.Search.Documents.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Search.CogServices;
using Search.Services;
using Search.Web.Models;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSearchController : SearchControllerBase
{
    private readonly IAcmeAutoCompleteService _autoCompleteService;
    private readonly IIndexConfigurationService _indexConfigurationService;
    private readonly IValidator<AcmeAutoCompleteQueryDto> _autoCompleteValidator;
    private readonly IAcmeSearchService _searchService;
    private readonly SearchServiceSettings _searchServiceSettings;
    private readonly IValidator<AcmeSearchQueryDto> _searchValidator;
    private readonly IAcmeSuggestService _suggestService;
    private readonly IValidator<AcmeSuggestQueryDto> _suggestValidator;

    /// <summary>Constructor</summary>
    public HotelSearchController(
        SearchServiceSettings searchServiceSettings,
        IValidator<AcmeSearchQueryDto> searchValidator,
        IValidator<AcmeSuggestQueryDto> suggestValidator,
        IValidator<AcmeAutoCompleteQueryDto> autoCompleteValidator,
        IAcmeSearchService searchService,
        IAcmeSuggestService suggestService,
        IAcmeAutoCompleteService autoCompleteService,
        IIndexConfigurationService indexConfigurationService)
    {
        _searchServiceSettings = searchServiceSettings;
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
        _autoCompleteValidator = autoCompleteValidator;
        _searchService = searchService;
        _suggestService = suggestService;
        _autoCompleteService = autoCompleteService;
        _indexConfigurationService = indexConfigurationService;
    }

    [HttpPost("AutoComplete")]
    public async Task<IActionResult> AutoComplete(AcmeAutoCompleteQueryDto queryDto, CancellationToken cancellationToken)
    {
        var validationResult = await _autoCompleteValidator.ValidateAsync(queryDto, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeAutoCompleteQuery
        {
            Filters = queryDto.Filters,
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IndexName = _searchServiceSettings.Hotel.IndexName,
            Mode = AutocompleteMode.OneTerm,
            NumberOfSuggestionsToRetrieve = queryDto.NumberOfSuggestionsToRetrieve,
            Query = queryDto.Query,
            SearchFields = queryDto.SearchFields,
            SuggestorName = _searchServiceSettings.Hotel.SuggestorName,
            UseFuzzyMatching = queryDto.UseFuzzyMatching
        };

        var result = await _autoCompleteService.AutoCompleteAsync(request,
            await GetSecurityTrimmingFieldAsync(_searchServiceSettings.Hotel.IndexName, cancellationToken), GetRoles());

        return Ok(result);
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search(AcmeSearchQueryDto queryDto, CancellationToken cancellationToken)
    {
        var validationResult = await _searchValidator.ValidateAsync(queryDto, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeSearchQuery
        {
            DocumentFields = queryDto.DocumentFields,
            DocumentFieldMaps = queryDto.DocumentFieldMaps,
            FacetFields = new List<string> { "baseRate", "category", "parkingIncluded", "rating", "smokingAllowed", "tags" },
            Filters = queryDto.Filters,
            HighlightFields = new List<string> { "hotelName", "category", "description" },
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IncludeAllWords = queryDto.IncludeAllWords,
            IncludeCount = queryDto.IncludeCount,
            IndexName = _searchServiceSettings.Hotel.IndexName,
            ItemsPerPage = queryDto.ItemsPerPage,
            OrderByFields = queryDto.OrderByFields,
            PageNumber = queryDto.PageNumber,
            Query = queryDto.Query,
            QueryType = queryDto.QueryType,
        };

        AcmeSearchQueryResult<SearchResult<SearchDocument>> searchResult = await _searchService.SearchAsync(request,
            await GetSecurityTrimmingFieldAsync(_searchServiceSettings.Hotel.IndexName, cancellationToken), GetRoles());

        return new OkObjectResult(searchResult);
    }

    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestQueryDto queryDto, CancellationToken cancellationToken)
    {
        var validationResult = await _suggestValidator.ValidateAsync(queryDto, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeSuggestQuery
        {
            DocumentFields = queryDto.DocumentFields,
            DocumentFieldMaps = queryDto.DocumentFieldMaps,
            Filters = queryDto.Filters,
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IndexName = _searchServiceSettings.Hotel.IndexName,
            NumberOfSuggestionsToRetrieve = queryDto.NumberOfSuggestionsToRetrieve,
            OrderByFields = queryDto.OrderByFields,
            Query = queryDto.Query,
            SearchFields = queryDto.SearchFields,
            SuggestorName = _searchServiceSettings.Hotel.SuggestorName,
            UseFuzzyMatching = queryDto.UseFuzzyMatching ?? false,
        };

        SuggestResults<SearchDocument> suggestResult = await _suggestService.SuggestAsync(request,
            await GetSecurityTrimmingFieldAsync(_searchServiceSettings.Hotel.IndexName, cancellationToken), GetRoles());

        return new OkObjectResult(suggestResult);
    }

    private async Task<string?> GetSecurityTrimmingFieldAsync(string indexName, CancellationToken cancellationToken)
    {
        var indexConfig = await _indexConfigurationService.GetOrCreateAsync(indexName, cancellationToken);
        return indexConfig.SecurityTrimmingField;
    }
}
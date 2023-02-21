using AutoMapper;
using Azure.Search.Documents.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Search.CogServices;
using Search.Web.Models;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSearchController : SearchControllerBase
{
    private readonly IAcmeAutoCompleteService _autoCompleteService;
    private readonly IValidator<AcmeAutoCompleteQueryDto> _autoCompleteValidator;
    private readonly IMapper _mapper;
    private readonly IAcmeSearchService _searchService;
    private readonly IValidator<AcmeSearchQueryDto> _searchValidator;
    private readonly IAcmeSuggestService _suggestService;
    private readonly IValidator<AcmeSuggestQueryDto> _suggestValidator;

    /// <summary>Constructor</summary>
    public HotelSearchController(
         IMapper mapper,
        IValidator<AcmeSearchQueryDto> searchValidator,
        IValidator<AcmeSuggestQueryDto> suggestValidator,
         IValidator<AcmeAutoCompleteQueryDto> autoCompleteValidator,
        IAcmeSearchService searchService,
        IAcmeSuggestService suggestService,
        IAcmeAutoCompleteService autoCompleteService)
    {
        _mapper = mapper;
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
        _autoCompleteValidator = autoCompleteValidator;
        _searchService = searchService;
        _suggestService = suggestService;
        _autoCompleteService = autoCompleteService;
    }

    [HttpPost("AutoComplete")]
    public async Task<IActionResult> AutoComplete(AcmeAutoCompleteQueryDto queryDto)
    {
        var validationResult = await _autoCompleteValidator.ValidateAsync(queryDto);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeAutoCompleteQuery
        {
            Filters = queryDto.Filters,
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IndexName = "hotels-idx",
            Mode = AutocompleteMode.OneTerm,
            NumberOfSuggestionsToRetrieve = queryDto.NumberOfSuggestionsToRetrieve,
            Query = queryDto.Query,
            SearchFields = queryDto.SearchFields,
            SuggestorName = "sg",
            UseFuzzyMatching = queryDto.UseFuzzyMatching
        };

        var result = await _autoCompleteService.AutoCompleteAsync(request, "roles", GetRoles());

        return Ok(result);
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search(AcmeSearchQueryDto queryDto)
    {
        var validationResult = await _searchValidator.ValidateAsync(queryDto);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeSearchQuery
        {
            FacetFields = new List<string> { "baseRate", "category", "parkingIncluded", "rating", "smokingAllowed", "tags" },
            Filters = queryDto.Filters,
            HighlightFields = new List<string> { "hotelName", "category", "description" },
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IncludeAllWords = queryDto.IncludeAllWords,
            IncludeCount = queryDto.IncludeCount,
            IndexName = "hotels-idx",
            ItemsPerPage = queryDto.ItemsPerPage,
            OrderByFields = queryDto.OrderByFields,
            PageNumber = queryDto.PageNumber,
            Query = queryDto.Query,
            QueryType = queryDto.QueryType,
        };

        AcmeSearchQueryResult<SearchResult<SearchDocument>> searchResult = await _searchService.SearchAsync(request, "roles", GetRoles());

        return new OkObjectResult(searchResult);
    }

    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestQueryDto queryDto)
    {
        var validationResult = await _suggestValidator.ValidateAsync(queryDto);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        var request = new AcmeSuggestQuery
        {
            DocumentFields = queryDto.DocumentFields,
            Filters = queryDto.Filters,
            HighlightPostTag = "</b>",
            HighlightPreTag = "<b>",
            IndexName = "hotels-idx",
            NumberOfSuggestionsToRetrieve = queryDto.NumberOfSuggestionsToRetrieve,
            OrderByFields = queryDto.OrderByFields,
            Query = queryDto.Query,
            SearchFields = queryDto.SearchFields,
            SuggestorName = "sg",
            UseFuzzyMatching = queryDto.UseFuzzyMatching ?? false,
        };

        SuggestResults<SearchDocument> suggestResult = await _suggestService.SuggestAsync(request, "roles", GetRoles());

        return new OkObjectResult(suggestResult);
    }
}
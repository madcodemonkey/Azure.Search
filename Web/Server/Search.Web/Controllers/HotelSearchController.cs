using AutoMapper;
using Azure;
using Azure.Search.Documents.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Search.CogServices;
using Search.Model;
using Search.Services;
using Search.Web.Models;

namespace Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelSearchController : ControllerBase
{
    private readonly IHotelAutoCompleteService _autoCompleteService;
   private readonly IHotelSuggestorService _hotelSuggestorService;
    private readonly IMapper _mapper;
    private readonly IValidator<AcmeSearchQueryDto> _searchValidator;
    private readonly IValidator<AcmeSuggestorQuery> _suggestValidator;
    private readonly IAcmeSearchService _searchService;

    /// <summary>Constructor</summary>
    public HotelSearchController(
        IHotelSuggestorService hotelSuggestorService,
        IHotelAutoCompleteService autoCompleteService, IMapper mapper,
        IValidator<AcmeSearchQueryDto> searchValidator,
        IValidator<AcmeSuggestorQuery> suggestValidator,
        IAcmeSearchService searchService)
    {
        _hotelSuggestorService = hotelSuggestorService;
        _autoCompleteService = autoCompleteService;
        _mapper = mapper;
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
        _searchService = searchService;
    }

    [HttpPost("AutoComplete")]
    public async Task<IActionResult> AutoComplete(AcmeSuggestorQuery query)
    {
        var validationResult = await _suggestValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        Response<AutocompleteResults> autoCompleteResult = await _autoCompleteService.AutoCompleteAsync(query, GetRoles());

        List<HotelAutocompleteDto> mapResult = _mapper.Map<List<HotelAutocompleteDto>>(autoCompleteResult.Value.Results);

        return new OkObjectResult(mapResult);
    }

    [HttpPost("Search")]
    public async Task<IActionResult> Search(AcmeSearchQueryDto queryDto)
    {
        var validationResult = await _searchValidator.ValidateAsync(queryDto);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        AcmeSearchQuery request = new AcmeSearchQuery()
        {
            FacetFields = new List<string>()
                { "baseRate", "category", "parkingIncluded", "rating", "smokingAllowed", "tags" },
            Filters = queryDto.Filters,
            HighlightFields = new List<string>() { "hotelName", "category", "description" },
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
            IncludeAllWords = queryDto.IncludeAllWords,
            IncludeCount = queryDto.IncludeCount,
            IndexName = "hotels-idx",
            ItemsPerPage = queryDto.ItemsPerPage,
            OrderByFields = queryDto.OrderByFields,
            PageNumber = queryDto.PageNumber,
            Query = queryDto.Query,
            QueryType = queryDto.UseSemanticSearch ? SearchQueryType.Semantic : SearchQueryType.Simple  // TODO: Surface query type
        };
        
        AcmeSearchQueryResult<SearchResult<SearchDocument>> searchResult = await _searchService.SearchAsync(request, "roles", GetRoles());
        
        return new OkObjectResult(searchResult);
    }

    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestorQuery query)
    {
        var validationResult = await _suggestValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        SuggestResults<HotelDocument> suggestResult = await _hotelSuggestorService.SuggestAsync(query, GetRoles());

        List<HotelSuggestorDto> mapResult = _mapper.Map<List<HotelSuggestorDto>>(suggestResult.Results);

        return new OkObjectResult(mapResult);
    }

    private List<string> GetRoles()
    {
        return new List<string>() { "admin" };
        //ClaimsPrincipal currentUser = this.User;
        //return currentUser.FindAll(ClaimTypes.Role).Select(s => s.Value.ToLower()).ToArray();
    }
}
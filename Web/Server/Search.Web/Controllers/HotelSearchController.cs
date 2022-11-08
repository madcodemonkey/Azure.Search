using AutoMapper;
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
    private readonly IHotelSearchService _hotelSearchService;
    private readonly IHotelSuggestorService _hotelSuggestorService;
    private readonly IMapper _mapper;
    private readonly IValidator<AcmeSearchQuery> _searchValidator;
    private readonly IValidator<AcmeSuggestQuery> _suggestValidator;

    /// <summary>Constructor</summary>
    public HotelSearchController(IHotelSearchService hotelSearchService,
        IHotelSuggestorService hotelSuggestorService, IMapper mapper,
        IValidator<AcmeSearchQuery> searchValidator,
        IValidator<AcmeSuggestQuery> suggestValidator)
    {
        _hotelSearchService = hotelSearchService;
        _hotelSuggestorService = hotelSuggestorService;
        _mapper = mapper;
        _searchValidator = searchValidator;
        _suggestValidator = suggestValidator;
    }


    [HttpPost("Suggest")]
    public async Task<IActionResult> Suggest(AcmeSuggestQuery query)
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

    [HttpPost("Search")]
    public async Task<IActionResult> Search(AcmeSearchQuery query)
    {
        var validationResult = await _searchValidator.ValidateAsync(query);

        if (validationResult.IsValid == false)
        {
            return new BadRequestObjectResult(validationResult.ToString());
        }

        // Reference to paging: https://docs.microsoft.com/en-us/azure/search/tutorial-csharp-paging#extend-your-app-with-numbered-paging
        // Note on how continuation is really used https://stackoverflow.com/questions/33826731/how-to-use-microsoft-azure-search-searchcontinuationtoken
        AcmeSearchQueryResult<SearchResult<HotelDocument>> searchResult = await _hotelSearchService.SearchAsync(query, GetRoles());

        AcmeSearchQueryResult<HotelDocumentDto>? mapResult = _mapper.Map<AcmeSearchQueryResult<HotelDocumentDto>>(searchResult);

        return new OkObjectResult(mapResult);
    }


    private List<string> GetRoles()
    {
        return new List<string>() { "admin" };
        //ClaimsPrincipal currentUser = this.User;
        //return currentUser.FindAll(ClaimTypes.Role).Select(s => s.Value.ToLower()).ToArray();
    }
}
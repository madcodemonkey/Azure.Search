using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : AcmeSearchServiceBase<HotelDocument>, IHotelSearchService
{
    private readonly SearchServiceSettings _settings;
    private readonly IHotelSearchHighlightService _highlightService;

    /// <summary>Constructor</summary>
    public HotelSearchService(SearchServiceSettings settings,
        IAcmeSearchIndexService acmeSearchIndexService,
        IHotelSearchHighlightService highlightService,
        IHotelFieldService hotelFieldService) : base(acmeSearchIndexService, hotelFieldService)
    {
        _settings = settings;
        _highlightService = highlightService;
    }

    protected override string IndexName => _settings.Hotel.IndexName;

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    public override async Task<AcmeSearchQueryResult<SearchResult<HotelDocument>>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        var result = await base.SearchAsync(request, rolesTheUserIsAssigned);

        // It takes a lot of text to get more than one highlight item back and see this double break.
        _highlightService.MapHighlightsOnToDocumentSeparatorStyle(result.Docs, "<br/><br/>");

        // Normally, you would want to increase the number of characters to something larger, but 20 allows you to see the impact of the clipping.
        //_highlightService.MapHighlightsOnToDocumentGoogleStyle(result.Docs, 20);
 
        return result;
    }
}
using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : AcmeSearchServiceBase<HotelDocument>, IHotelSearchService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelSearchService(SearchServiceSettings settings,
        IAcmeSearchIndexService acmeSearchIndexService,
        IHotelFieldService hotelFieldService) : base(acmeSearchIndexService, hotelFieldService)
    {
        _settings = settings;
    }

    protected override string IndexName => _settings.Hotel.IndexName;

    /// <summary>Searches using the Azure Search API.</summary>
    /// <param name="request">The request from the user.</param>
    /// <param name="rolesTheUserIsAssigned">The roles assigned to the user</param>
    public override async Task<AcmeSearchQueryResult<SearchResult<HotelDocument>>> SearchAsync(AcmeSearchQuery request, List<string> rolesTheUserIsAssigned)
    {
        var result = await base.SearchAsync(request, rolesTheUserIsAssigned);

        MapHighlightsOnToDocument(result.Docs, nameof(HotelDocument.HotelName), nameof(HotelDocument.Category), nameof(HotelDocument.Description));

        return result;
    }
}
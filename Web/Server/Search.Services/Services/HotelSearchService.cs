using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : AcmeSearchServiceBase<SearchHotel, SearchHotel>, IHotelSearchService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>Constructor</summary>
    public HotelSearchService(SearchServiceSettings settings,
        IAcmeSearchIndexService acmeSearchIndexService,
        IHotelFilterService hotelFilterService) : base(acmeSearchIndexService, hotelFilterService)
    {
        _settings = settings;
    }

    protected override string IndexName => _settings.Hotel.IndexName;


    /// <summary>Converts a item (TInputClass) that was found by calling the Azure Search API into
    /// a desired return class (TResultClass).</summary>
    /// <param name="azSearchDocument">The item to convert</param>
    protected override SearchHotel ConvertOneDocument(SearchResult<SearchHotel> azSearchDocument)
    {
        return azSearchDocument.Document;
    }
}
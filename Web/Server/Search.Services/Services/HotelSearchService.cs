using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSearchService : AcmeSearchServiceBase<HotelDocument, HotelDocument>, IHotelSearchService
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


    /// <summary>Converts a item (TInputClass) that was found by calling the Azure Search API into
    /// a desired return class (TResultClass).</summary>
    /// <param name="azSearchDocument">The item to convert</param>
    protected override HotelDocument ConvertOneDocument(SearchResult<HotelDocument> azSearchDocument)
    {
        return azSearchDocument.Document;
    }
}
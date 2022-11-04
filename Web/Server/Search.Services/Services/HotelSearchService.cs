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

}
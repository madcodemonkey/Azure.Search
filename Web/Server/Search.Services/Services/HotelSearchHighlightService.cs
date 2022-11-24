using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSearchHighlightService : AcmeSearchHighlightServiceBase<HotelDocument>, IHotelSearchHighlightService
{
    public HotelSearchHighlightService(IHotelFieldService fieldService) : base(fieldService)
    {
    }
}
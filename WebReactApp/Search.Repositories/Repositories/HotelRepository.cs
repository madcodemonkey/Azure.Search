using Search.Model;

namespace Search.Repositories;

public class HotelRepository : RepositoryPrimaryKeyBase<Hotel, AcmeContext, int>, IHotelRepository
{
    /// <summary>Constructor</summary>
    public HotelRepository(AcmeContext context) : base(context)
    {

    }
}
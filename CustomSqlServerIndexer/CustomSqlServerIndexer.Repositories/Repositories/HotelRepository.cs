using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Repositories;

public class HotelRepository : RepositoryPrimaryKeyBase<Hotel, CustomSqlServerContext, int>, IHotelRepository
{
    /// <summary>Constructor</summary>
    public HotelRepository(CustomSqlServerContext context) : base(context)
    {

    }
}
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public interface IGremlinDataService
{
    Task CreateAllAsync(CancellationToken cancellationToken);
    Task DeleteAllAsync(CancellationToken cancellationToken);
    Task<List<Person>> GetPeopleAsync(CancellationToken cancellationToken);
}
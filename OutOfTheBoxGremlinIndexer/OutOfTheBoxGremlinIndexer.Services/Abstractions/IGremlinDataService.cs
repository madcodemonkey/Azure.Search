using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public interface IGremlinDataService
{
    Task CreateAllAsync();
    Task DeleteAllAsync();
    Task<List<Person>> GetPeopleAsync();
}
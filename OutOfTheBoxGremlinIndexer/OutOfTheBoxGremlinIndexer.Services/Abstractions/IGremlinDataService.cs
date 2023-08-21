using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public interface IGremlinDataService
{
    Task CreateAllAsync(CancellationToken cancellationToken);
    Task<bool> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken);

    Task DeleteAllAsync(CancellationToken cancellationToken);
    Task<List<Person>> GetPeopleAsync(CancellationToken cancellationToken);
    Task<bool> CreateKnowsRelationship(string id1, string id2);
}
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Services;

public interface IGremlinDataService
{
    Task CreateAllAsync(CancellationToken cancellationToken);
    Task<bool> CreateKnowsRelationship(string id1, string id2);

    Task<bool> CreatePersonAsync(Person newPerson, CancellationToken cancellationToken);
    Task DeleteAllAsync(CancellationToken cancellationToken);

    Task<bool> DeletePersonAsync(string id, CancellationToken cancellationToken);
    Task<List<Person>> GetPeopleAsync(bool showSoftDeleted = false, CancellationToken cancellationToken = default);
}
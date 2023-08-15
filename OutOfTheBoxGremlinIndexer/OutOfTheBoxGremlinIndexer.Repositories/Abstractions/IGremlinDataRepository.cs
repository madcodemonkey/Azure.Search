using CustomSqlServerIndexer.Models;
using Gremlin.Net.Driver;

namespace CustomSqlServerIndexer.Repositories;

public interface IGremlinDataRepository
{
    Task<ResultSet<dynamic>> DeleteAllAsync();
    Task<ResultSet<dynamic>> CreatePersonAsync(string id, string firstname, string lastName, int age, bool isDeleted);
    Task<ResultSet<dynamic>> CreateKnowsRelationshipAsync(string id1, string id2);

    Task<List<Person>> GetPeopleAsync();


}
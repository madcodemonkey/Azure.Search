using CustomSqlServerIndexer.Models;
using Gremlin.Net.Driver;

namespace CustomSqlServerIndexer.Repositories;

public interface IGremlinDataRepository
{
    /// <summary>
    /// Creates a "knows" relationship between two people
    /// </summary>
    /// <param name="id1">The first person's id</param>
    /// <param name="id2">The second person's id</param>
    Task<ResultSet<dynamic>> CreateKnowsRelationshipAsync(string id1, string id2);

    /// <summary>
    /// Creates a new person
    /// </summary>
    /// <param name="id">The person id</param>
    /// <param name="firstname">first name</param>
    /// <param name="lastName">last name</param>
    /// <param name="age">age</param>
    /// <param name="isDeleted">The soft delete field (by default should be false)</param>
    Task<ResultSet<dynamic>> CreatePersonAsync(string id, string firstname, string lastName, int age, bool isDeleted = false);

    /// <summary>
    /// Deletes all the data
    /// </summary>
    Task<ResultSet<dynamic>> DeleteAllAsync();

    /// <summary>
    /// Does a soft delete on a person
    /// </summary>
    /// <param name="id">The id of the person</param>
    /// <param name="softDelete">true just sets the isDelete flag to true; otherwise, false really delete the record!</param>
    Task<bool> DeletePersonAsync(string id, bool softDelete = true);


    /// <summary>
    /// Gets all the people in the Graph database.
    /// </summary>
    Task<List<Person>> GetPeopleAsync(bool showSoftDeleted = false);

    /// <summary>
    /// Checks to see if a person exists in the database.
    /// </summary>
    /// <param name="id">The persons id</param>
    Task<bool> PersonExistsAsync(string id);
}
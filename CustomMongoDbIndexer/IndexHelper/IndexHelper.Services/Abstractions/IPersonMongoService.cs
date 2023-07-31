using IndexHelper.Models;
using MongoDBServices;

namespace IndexHelper.Services;

public interface IPersonMongoService
{
    Task CreateAsync(PersonModel person, CancellationToken cancellationToken = default);
    Task DeleteAsync(string personId, CancellationToken cancellationToken = default);
    Task<MongoPage<PersonModel>> GetAllAsync(int pageNumber, int itemsPerPage, CancellationToken cancellationToken = default);
    Task UpdateAsync(PersonModel person, CancellationToken cancellationToken = default);
}
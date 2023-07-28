using IndexHelper.Models;

namespace IndexHelper.Services;

public interface IPersonMongoService
{
    Task CreateAsync(PersonModel person);
    Task DeleteAsync(string personId);
    Task<List<PersonModel>> GetAllAsync();
    Task UpdateAsync(PersonModel person);
}
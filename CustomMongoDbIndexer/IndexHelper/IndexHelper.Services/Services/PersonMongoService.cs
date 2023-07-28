using IndexHelper.Models;
using IndexHelper.Repositories;

namespace IndexHelper.Services;

public class PersonMongoService : IPersonMongoService
{
    private readonly IPersonRepository _personRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    public PersonMongoService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task CreateAsync(PersonModel person)
    {
        await _personRepository.CreateAsync(person);
    }

    public async Task DeleteAsync(string personId)
    {
        await _personRepository.DeleteAsync(personId);
    }

    public async Task<List<PersonModel>> GetAllAsync()
    {
        return await _personRepository.GetAllAsync();
    }

    public async Task UpdateAsync(PersonModel person)
    {
        await _personRepository.UpdateAsync(person);
    }
}

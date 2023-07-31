using IndexHelper.Models;
using IndexHelper.Repositories;
using MongoDBServices;

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

    public async Task CreateAsync(PersonModel person, CancellationToken cancellationToken = default)
    {
        await _personRepository.CreateAsync(person, cancellationToken);
    }

    public async Task DeleteAsync(string personId, CancellationToken cancellationToken = default)
    {
        await _personRepository.DeleteAsync(personId, cancellationToken);
    }

    public async Task<MongoPage<PersonModel>> GetAllAsync(int pageNumber, int itemsPerPage, CancellationToken cancellationToken = default)
    {
        return await _personRepository.GetAllAsync(pageNumber, itemsPerPage, cancellationToken);
    }

    public async Task UpdateAsync(PersonModel person, CancellationToken cancellationToken = default)
    {
        await _personRepository.UpdateAsync(person, cancellationToken);
    }
}

using IndexHelper.Models;
using MongoDBServices;

namespace IndexHelper.Repositories;

public interface IPersonRepository : IMongoCollectionRepository<PersonModel>
{
}
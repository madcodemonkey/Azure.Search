using IndexHelper.Models;
using MongoDBServices;

namespace IndexHelper.Repositories
{
    public class PersonRepository : MongoCollectionBaseRepository<PersonModel>, IPersonRepository
    {
        private readonly IndexAppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonRepository(IndexAppSettings appSettings, IMongoClientService clientService) : base(clientService)
        {
            _appSettings = appSettings;
        }

        protected override string DatabaseName => _appSettings.MongoDatabaseName;
        protected override string CollectionName => _appSettings.MongoPeopleCollection;
    }
}
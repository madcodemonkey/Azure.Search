namespace MongoDBServices;

public interface IMongoCollectionRepository<T> where T : MongoEntity
{
    Task CreateAsync(T mongoEntity);
    Task DeleteAsync(T mongoEntity);
    Task DeleteAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task UpdateAsync(T mongoEntity);
    Task<bool> ExistsAsync(string id);
}
namespace WorkerServiceMongoIndexer.Services;

public class MongoResumeTokenService : IMongoResumeTokenService
{
    private string? _token;

    public string? LoadResumeToken()
    {
        // TODO: Retrieve from somewhere (e.g., Redis, database, etc.)
        return _token;
    }

    public void SaveResumeToken(string? token)
    {
        // TODO: Store somewhere (e.g., Redis, database, etc.)
        _token = token;
    }
}
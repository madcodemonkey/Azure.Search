namespace CustomMongoDbIndexer.Services;

public interface IMongoResumeTokenService
{
    string? LoadResumeToken();

    void SaveResumeToken(string? token);
}
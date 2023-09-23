namespace Search.Repositories;

/// <summary>Settings used by the repository library.</summary>
public class AcmeDatabaseOptions
{
    public string ConnectionString { get; set; }
    public bool RunMigrationsOnStartup { get; set; } = false;
}
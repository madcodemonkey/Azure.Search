namespace Search.Repositories;

/// <summary>Settings used by the repository library.</summary>
public class AcmeDatabaseOptions
{
    public bool RunMigrationsOnStartup { get; set; } = false;

    public string ConnectionString { get; set; }
}
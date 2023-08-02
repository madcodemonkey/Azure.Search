namespace CustomSqlServerIndexer.Services;

/// <summary>Settings used by the repository library.</summary>
public class RepositorySettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool RunMigrationsOnStartup { get; set; } = false;
}
using CustomSqlServerIndexer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
/// This is only needed when EF migration command
/// 
/// You'll need to put the database connection in local.settings.json because it will not get pulled from secrets.
/// If that still doesn't work, put the connection string directly on line 17
/// </summary>
public class CustomSqlServerContextContextFactory : IDesignTimeDbContextFactory<CustomSqlServerContext>
{
    public CustomSqlServerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CustomSqlServerContext>();

        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DatabaseConnectionString") ?? string.Empty);

        return new CustomSqlServerContext(optionsBuilder.Options);
    }
}
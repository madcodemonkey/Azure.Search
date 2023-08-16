using Microsoft.EntityFrameworkCore;
using CustomSqlServerIndexer.Models;

namespace CustomSqlServerIndexer.Repositories;

public class CustomSqlServerContext : DbContext
{
    public CustomSqlServerContext(DbContextOptions<CustomSqlServerContext> options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}


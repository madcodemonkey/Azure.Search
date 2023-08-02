using Microsoft.EntityFrameworkCore;
using CustomSqlServerIndexer.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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


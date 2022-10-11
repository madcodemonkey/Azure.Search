using Microsoft.EntityFrameworkCore;
using Search.Model;

namespace Search.Repositories;

public class AcmeContext : DbContext
{
    public AcmeContext(DbContextOptions<AcmeContext> options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
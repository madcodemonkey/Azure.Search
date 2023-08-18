using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Search.Model;

namespace Search.Repositories;

internal class IndexConfigurationConfiguration : IEntityTypeConfiguration<IndexConfiguration>
{
    public void Configure(EntityTypeBuilder<IndexConfiguration> builder)
    {
        builder.ToTable("IndexConfiguration");

        builder.HasKey(b => b.IndexName);
        builder.Property(b => b.UsesCamelCaseFieldNames).IsRequired(); 
    }
}
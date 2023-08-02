using CustomSqlServerIndexer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 

namespace CustomSqlServerIndexer.Repositories;

internal class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotel");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.DescriptionFr).HasColumnName("Description_fr");
        builder.Property(b => b.HotelName).IsRequired();
        builder.Property(b => b.Roles).IsRequired();
    }
}
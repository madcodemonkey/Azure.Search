using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Search.Model;

namespace Search.Repositories;

internal class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotel");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Description).HasColumnName("Description_fr");
        builder.Property(b => b.HotelName).IsRequired();
        builder.Property(b => b.Roles).IsRequired();
    }
}
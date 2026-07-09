using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Restaurants;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class RestaurantImageConfiguration : IEntityTypeConfiguration<RestaurantImage>
{
    public void Configure(EntityTypeBuilder<RestaurantImage> builder)
    {
        builder.ToTable("RestaurantImages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ImageType).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Images).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.RestaurantId, x.ImageType });
    }
}
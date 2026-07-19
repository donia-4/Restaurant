using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Restaurants;
using Restaurant.Domain.Restaurants.Enums;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant.Domain.Restaurants.Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant.Domain.Restaurants.Restaurant> builder)
    {
        builder.ToTable("Restaurants");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Logo).HasMaxLength(500);
        builder.Property(x => x.LogoPublicId).HasMaxLength(500);
        builder.Property(x => x.CoverImage).HasMaxLength(500);
        builder.Property(x => x.CoverImagePublicId).HasMaxLength(500);
        builder.Property(x => x.CuisineType).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.AverageRating).HasPrecision(3, 2);
        builder.Property(x => x.IsApproved).HasDefaultValue(false);

        // Relationships
        builder.HasMany(x => x.Branches).WithOne(x => x.Restaurant).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Categories).WithOne(x => x.Restaurant).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Foods).WithOne(x => x.Restaurant).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Images).WithOne(x => x.Restaurant).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Reviews).WithOne(x => x.Restaurant).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        // Indexes
        builder.HasIndex(x => x.OwnerId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CuisineType);
        builder.HasIndex(x => new { x.Name, x.Status, x.IsApproved });
    }
}
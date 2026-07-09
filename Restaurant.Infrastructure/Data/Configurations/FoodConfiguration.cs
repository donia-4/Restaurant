using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Foods;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class FoodConfiguration : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.ToTable("Foods");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Price).HasPrecision(18, 2);
        builder.Property(x => x.Image).HasMaxLength(500);
        builder.Property(x => x.PreparationTimeMinutes).HasDefaultValue(0);
        builder.Property(x => x.IsAvailable).HasDefaultValue(true);
        builder.Property(x => x.IsVisible).HasDefaultValue(true);

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Foods).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Category).WithMany(x => x.Foods).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict); // BR-04
        builder.HasMany(x => x.AddOns).WithOne(x => x.Food).HasForeignKey(x => x.FoodId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RestaurantId);
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => new { x.RestaurantId, x.Name });
        builder.HasIndex(x => x.IsAvailable);
        builder.HasIndex(x => x.IsVisible);
    }
}
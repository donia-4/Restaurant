using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Categories;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Categories).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Foods).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict); // BR-04

        builder.HasIndex(x => new { x.RestaurantId, x.Name }).IsUnique();
        builder.HasIndex(x => new { x.RestaurantId, x.DisplayOrder });
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Reviews;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class ReviewConfiguration
    : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(1000);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasOne(x => x.Restaurant)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RestaurantId);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => new
        {
            x.RestaurantId,
            x.UserId,
            x.IsDeleted
        }).IsUnique();
    }
}
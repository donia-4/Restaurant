using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Branches;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Latitude).HasPrecision(18, 8);
        builder.Property(x => x.Longitude).HasPrecision(18, 8);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.Restaurant).WithMany(x => x.Branches).HasForeignKey(x => x.RestaurantId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.WorkingHours).WithOne(x => x.Branch).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.DeliveryZones).WithOne(x => x.Branch).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RestaurantId);
        builder.HasIndex(x => new { x.Latitude, x.Longitude });
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.DeliveryZones;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class DeliveryZoneConfiguration : IEntityTypeConfiguration<DeliveryZone>
{
    public void Configure(EntityTypeBuilder<DeliveryZone> builder)
    {
        builder.ToTable("DeliveryZones");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ZoneName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.DeliveryFee).HasPrecision(18, 2);
        builder.Property(x => x.MinimumOrder).HasPrecision(18, 2);
        builder.Property(x => x.PolygonGeoJson).HasMaxLength(4000);

        builder.HasOne(x => x.Branch).WithMany(x => x.DeliveryZones).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.BranchId);
    }
}
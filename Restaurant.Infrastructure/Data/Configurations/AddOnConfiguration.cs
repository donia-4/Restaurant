using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.AddOns;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class AddOnConfiguration : IEntityTypeConfiguration<AddOn>
{
    public void Configure(EntityTypeBuilder<AddOn> builder)
    {
        builder.ToTable("AddOns");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Price).HasPrecision(18, 2);
        builder.Property(x => x.IsRequired).HasDefaultValue(false);

        builder.HasOne(x => x.Food).WithMany(x => x.AddOns).HasForeignKey(x => x.FoodId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.FoodId);
    }
}
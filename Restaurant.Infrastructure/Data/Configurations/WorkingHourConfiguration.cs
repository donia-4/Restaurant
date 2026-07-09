using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.WorkingHours;

namespace Restaurant.Infrastructure.Data.Configurations;

public sealed class WorkingHourConfiguration : IEntityTypeConfiguration<WorkingHour>
{
    public void Configure(EntityTypeBuilder<WorkingHour> builder)
    {
        builder.ToTable("WorkingHours");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DayOfWeek).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(x => x.IsClosed).HasDefaultValue(false);

        builder.HasOne(x => x.Branch).WithMany(x => x.WorkingHours).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.BranchId, x.DayOfWeek }).IsUnique();
    }
}
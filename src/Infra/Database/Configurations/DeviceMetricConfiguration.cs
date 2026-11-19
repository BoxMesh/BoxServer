using Zin.Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zin.Infra.Database.Configurations;

public class DeviceMetricConfiguration : IEntityTypeConfiguration<DeviceMetric>
{
    public void Configure(EntityTypeBuilder<DeviceMetric> builder)
    {
        builder.ToTable("DeviceMetric");
        builder.HasKey(x => x.Id);
        // Every read is one device over a time window.
        builder.HasIndex(x => new { x.DeviceId, x.At });
        builder.HasOne<Device>().WithMany().HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);
    }
}

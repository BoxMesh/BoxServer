using BoxServer.Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxServer.Infra.Database.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Device");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OSName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.OSVersion)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.OSArchitecture)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.OSArchitecture)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.IPAddress)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(x => x.MacAddress)
            .HasMaxLength(17)
            .IsRequired();

        builder.Property(x => x.CPU)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.RAM)
            .IsRequired();

        builder.Property(x => x.Disk)
            .IsRequired();

        builder.Property(x => x.Uptime)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);
    }
}
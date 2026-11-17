using BoxServer.Infra.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoxServer.Infra.Database.Configurations;

// Rows go away with either side: deleting a group, device or user cleans up its memberships and grants.
public class DeviceGroupConfiguration : IEntityTypeConfiguration<DeviceGroup>
{
    public void Configure(EntityTypeBuilder<DeviceGroup> builder)
    {
        builder.ToTable("DeviceGroup");
        builder.HasKey(x => new { x.DeviceId, x.GroupId });
        builder.HasIndex(x => x.GroupId);
        builder.HasOne<Device>().WithMany().HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Group>().WithMany().HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserGroupAccessConfiguration : IEntityTypeConfiguration<UserGroupAccess>
{
    public void Configure(EntityTypeBuilder<UserGroupAccess> builder)
    {
        builder.ToTable("UserGroupAccess");
        builder.HasKey(x => new { x.UserId, x.GroupId });
        builder.HasIndex(x => x.GroupId);
        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Group>().WithMany().HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserDeviceAccessConfiguration : IEntityTypeConfiguration<UserDeviceAccess>
{
    public void Configure(EntityTypeBuilder<UserDeviceAccess> builder)
    {
        builder.ToTable("UserDeviceAccess");
        builder.HasKey(x => new { x.UserId, x.DeviceId });
        builder.HasIndex(x => x.DeviceId);
        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Device>().WithMany().HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);
    }
}

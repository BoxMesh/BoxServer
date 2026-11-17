namespace BoxServer.Infra.Database.Entities;

/// <summary>A device is in a group; a device can be in several.</summary>
public class DeviceGroup
{
    public Guid DeviceId { get; set; }
    public Guid GroupId { get; set; }
}

/// <summary>An Operator may see every device in this group, including ones added to it later.</summary>
public class UserGroupAccess
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
}

/// <summary>An Operator may see this one device.</summary>
public class UserDeviceAccess
{
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
}

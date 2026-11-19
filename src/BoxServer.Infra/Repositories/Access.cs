using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Zin.Infra.Repository;

/// <summary>What a User was granted: whole groups and single devices.</summary>
public record UserAccessIds(Guid[] GroupIds, Guid[] DeviceIds);

public class AccessRepository(DBContext _context)
{
    // A device is visible through a grant on it, or on any group it is in.
    private IQueryable<Device> VisibleTo(Guid userId) =>
        _context.Set<Device>().Where(d =>
            _context.Set<UserDeviceAccess>().Any(a => a.UserId == userId && a.DeviceId == d.Id)
            || _context.Set<DeviceGroup>().Any(m => m.DeviceId == d.Id
                && _context.Set<UserGroupAccess>().Any(g => g.UserId == userId && g.GroupId == m.GroupId)));

    public async Task<List<Device>> DevicesFor(Guid userId) => await VisibleTo(userId).ToListAsync();

    public async Task<bool> CanSee(Guid userId, Guid deviceId) => await VisibleTo(userId).AnyAsync(d => d.Id == deviceId);

    public async Task<Dictionary<Guid, Guid[]>> GroupIdsByDevice()
    {
        List<DeviceGroup> rows = await _context.Set<DeviceGroup>().ToListAsync();
        return rows.GroupBy(m => m.DeviceId).ToDictionary(g => g.Key, g => g.Select(m => m.GroupId).ToArray());
    }

    public async Task<Guid[]> GroupIdsOf(Guid deviceId) =>
        await _context.Set<DeviceGroup>().Where(m => m.DeviceId == deviceId).Select(m => m.GroupId).ToArrayAsync();

    /// <summary>Replaces the device's groups with exactly these.</summary>
    public async Task SetDeviceGroups(Guid deviceId, IEnumerable<Guid> groupIds)
    {
        _context.Set<DeviceGroup>().RemoveRange(_context.Set<DeviceGroup>().Where(m => m.DeviceId == deviceId));
        _context.Set<DeviceGroup>().AddRange(groupIds.Distinct().Select(g => new DeviceGroup { DeviceId = deviceId, GroupId = g }));
        await _context.SaveChangesAsync();
    }

    public async Task<UserAccessIds> AccessOf(Guid userId) => new(
        await _context.Set<UserGroupAccess>().Where(a => a.UserId == userId).Select(a => a.GroupId).ToArrayAsync(),
        await _context.Set<UserDeviceAccess>().Where(a => a.UserId == userId).Select(a => a.DeviceId).ToArrayAsync());

    public async Task<Dictionary<Guid, UserAccessIds>> AccessByUser()
    {
        List<UserGroupAccess> groups = await _context.Set<UserGroupAccess>().ToListAsync();
        List<UserDeviceAccess> devices = await _context.Set<UserDeviceAccess>().ToListAsync();
        return groups.Select(a => a.UserId).Union(devices.Select(a => a.UserId)).ToDictionary(
            id => id,
            id => new UserAccessIds(
                groups.Where(a => a.UserId == id).Select(a => a.GroupId).ToArray(),
                devices.Where(a => a.UserId == id).Select(a => a.DeviceId).ToArray()));
    }

    /// <summary>Replaces the user's grants with exactly these.</summary>
    public async Task SetAccess(Guid userId, IEnumerable<Guid> groupIds, IEnumerable<Guid> deviceIds)
    {
        _context.Set<UserGroupAccess>().RemoveRange(_context.Set<UserGroupAccess>().Where(a => a.UserId == userId));
        _context.Set<UserDeviceAccess>().RemoveRange(_context.Set<UserDeviceAccess>().Where(a => a.UserId == userId));
        _context.Set<UserGroupAccess>().AddRange(groupIds.Distinct().Select(g => new UserGroupAccess { UserId = userId, GroupId = g }));
        _context.Set<UserDeviceAccess>().AddRange(deviceIds.Distinct().Select(d => new UserDeviceAccess { UserId = userId, DeviceId = d }));
        await _context.SaveChangesAsync();
    }

    /// <summary>True when every one of these groups and devices exists.</summary>
    public async Task<bool> AllExist(IEnumerable<Guid> groupIds, IEnumerable<Guid> deviceIds)
    {
        List<Guid> groups = groupIds.Distinct().ToList();
        List<Guid> devices = deviceIds.Distinct().ToList();
        return await _context.Set<Group>().CountAsync(g => groups.Contains(g.Id)) == groups.Count
            && await _context.Set<Device>().CountAsync(d => devices.Contains(d.Id)) == devices.Count;
    }
}

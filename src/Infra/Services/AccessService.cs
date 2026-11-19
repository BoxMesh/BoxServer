using Zin.Infra.Database.Entities;
using Zin.Infra.Repository;

namespace Zin.Infra.Service;

/// <summary>Who may see which device: group membership of devices, and what each User was granted.</summary>
public class AccessService(AccessRepository accessRepository)
{
    public Task<List<Device>> DevicesFor(Guid userId) => accessRepository.DevicesFor(userId);
    public Task<bool> CanSee(Guid userId, Guid deviceId) => accessRepository.CanSee(userId, deviceId);
    public Task<Dictionary<Guid, Guid[]>> GroupIdsByDevice() => accessRepository.GroupIdsByDevice();
    public Task<Guid[]> GroupIdsOf(Guid deviceId) => accessRepository.GroupIdsOf(deviceId);
    public Task SetDeviceGroups(Guid deviceId, IEnumerable<Guid> groupIds) => accessRepository.SetDeviceGroups(deviceId, groupIds);
    public Task<UserAccessIds> AccessOf(Guid userId) => accessRepository.AccessOf(userId);
    public Task<Dictionary<Guid, UserAccessIds>> AccessByUser() => accessRepository.AccessByUser();
    public Task SetAccess(Guid userId, IEnumerable<Guid> groupIds, IEnumerable<Guid> deviceIds) => accessRepository.SetAccess(userId, groupIds, deviceIds);
    public Task<bool> AllExist(IEnumerable<Guid> groupIds, IEnumerable<Guid> deviceIds) => accessRepository.AllExist(groupIds, deviceIds);
}

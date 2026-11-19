using Zin.API.GRPC;
// Only this type: the DTO namespace has its own DeviceResponse, which would clash with the gRPC one.
using ProcessResponse = Zin.API.DTO.Devices.ProcessResponse;
using Grpc.Core;

using Google.Protobuf.WellKnownTypes;
using Zin.Infra.Database.Entities;
using Zin.Infra.Service;

public class GDeviceService(DeviceService deviceService) : DeviceGRPC.DeviceGRPCBase
{
    // shortcut: any caller that can reach the port may report; check an enrollment token once agents enroll.
    public override async Task<DeviceResponse> Ping(DeviceRequest device,ServerCallContext context)
    {
        if (!Guid.TryParse(device.Id, out Guid id) || string.IsNullOrWhiteSpace(device.HostName))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "id must be a UUID and host_name must be set"));
        }
        Device? dPing = await deviceService.Ping(new Device
        {
            Id = id,
            Hostname=device.HostName,
            OSName=device.OsName,
            OSVersion=device.OsVersion,
            OSArchitecture=device.OsArchitecture,
            IPAddress=device.IpAddress,
            MacAddress=device.MacAddress,
            CPU=device.Cpu,
            RAM=device.Ram,
            Disk=device.Disk,
            Uptime=device.Uptime,
            AgentVersion=device.AgentVersion,
            CpuUsage=device.CpuUsage,
            RamUsage=device.RamUsage,
            DiskUsage=device.DiskUsage,
            NetIn=device.NetIn,
            NetOut=device.NetOut,
            Processes=ProcessResponse.ToJson(device.Processes.Select(p => new ProcessResponse(p.Pid, p.Name, p.User, p.Status, p.Cpu, p.MemoryMb))),
        });
        return new DeviceResponse{Data = Any.Pack(new BoolValue { Value = dPing != null })};
    }
}

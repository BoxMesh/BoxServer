using BoxServer.API.GRPC;
using Grpc.Core;

using Google.Protobuf.WellKnownTypes;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

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
        });
        return new DeviceResponse{Data = Any.Pack(new BoolValue { Value = dPing != null })};
    }
}

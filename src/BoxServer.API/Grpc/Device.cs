using BoxServer.API.GRPC;
using Grpc.Core;

using Google.Protobuf.WellKnownTypes;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

public class GDeviceService(DeviceService deviceService) : DeviceGRPC.DeviceGRPCBase
{
    public override async Task<DeviceResponse> Ping(DeviceRequest device,ServerCallContext context)
    {
        Device? dPing = await deviceService.Ping(new Device
        {
            Id = Guid.Parse(device.Id),
            Hostname=device.HostName,
            OSName=device.OsName,
            OSVersion=device.OsVersion,
            OSArchitecture=device.OsArchitecture,
            IPAddress=device.IpAddress,
            MacAddress=device.MacAddress,
            CPU=device.Cpu,
            RAM=device.Ram,
            Disk=device.Disk,
            Uptime=device.Uptime
        });
        Any Data = Any.Pack(new BoolValue { Value = false });
        if (dPing != null)
        {
            Data = Any.Pack(new BoolValue { Value = false });
        };
        return new DeviceResponse{Data = Data};
    }
}
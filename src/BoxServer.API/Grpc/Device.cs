using BoxServer.API.GRPC;
using Grpc.Core;

using Google.Protobuf.WellKnownTypes;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

public class GDeviceService(DeviceService deviceService) : DeviceGRPC.DeviceGRPCBase
{
    public override async Task<DeviceResponse> Ping(DeviceRequest device,ServerCallContext context)
    {
        Device dPing = await deviceService.Ping(new Device
        {
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
        return new DeviceResponse
        {
            Data = Any.Pack(new StringValue { Value = dPing.Id.ToString() })
        };
    }
}
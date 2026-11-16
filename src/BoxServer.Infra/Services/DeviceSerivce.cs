using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

using Microsoft.AspNetCore.Http.Features;

namespace BoxServer.Infra.Service
{
    public class DeviceService(DeviceRepository deviceRepository, DeviceMetricRepository metricRepository)
    {
        public async Task<List<Device>> GetDevices()
        {
            return await deviceRepository.FindMany();
        }
        public async Task<Device?> GetByHostname(string hostname)
        {
            return await deviceRepository.FindBy("Hostname", hostname);
        }
        public async Task<Device?> Ping(Device device)
        {
            Device? saved;
            Device? record = await deviceRepository.FindByID(device.Id);
            if (record == null)
            {
                saved = await deviceRepository.Create(device);
            }
            else
            {
                // Update copies every field, so carry over when the device enrolled and stamp when it was last heard from.
                device.CreatedAt = record.CreatedAt;
                device.CreatedBy = record.CreatedBy;
                device.UpdatedAt = DateTime.UtcNow;
                saved = await deviceRepository.Update(device.Id,device);
            }
            if (saved != null)
            {
                await metricRepository.Add(new DeviceMetric
                {
                    DeviceId = saved.Id,
                    At = DateTime.UtcNow,
                    Cpu = device.CpuUsage,
                    Ram = device.RamUsage,
                    Disk = device.DiskUsage,
                    NetIn = device.NetIn,
                    NetOut = device.NetOut,
                });
            }
            return saved;
        }
        public async Task<List<MetricPoint>> GetMetrics(Guid deviceId, DateTime from, TimeSpan step)
        {
            return await metricRepository.Series(deviceId, from, step);
        }
    }
}

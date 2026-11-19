using Zin.Infra.Database.Entities;
using Zin.Infra.Repository;

using Microsoft.AspNetCore.Http.Features;

namespace Zin.Infra.Service
{
    public class DeviceService(DeviceRepository deviceRepository, DeviceMetricRepository metricRepository)
    {
        public async Task<List<Device>> GetDevices()
        {
            return await deviceRepository.FindMany();
        }
        public async Task<Device?> GetById(Guid id)
        {
            return await deviceRepository.FindByID(id);
        }
        /// <summary>Renames the device in the console; leaves UpdatedAt alone, since that marks when the agent last reported.</summary>
        public async Task<Device?> SetAlias(Guid id, string alias)
        {
            Device? record = await deviceRepository.FindByID(id);
            if (record == null)
            {
                return null;
            }
            record.Alias = alias;
            return await deviceRepository.Update(id, record);
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
                // Update copies every field, so carry over what the agent doesn't send (when the device enrolled,
                // the console's alias) and stamp when it was last heard from.
                device.CreatedAt = record.CreatedAt;
                device.CreatedBy = record.CreatedBy;
                device.Alias = record.Alias;
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

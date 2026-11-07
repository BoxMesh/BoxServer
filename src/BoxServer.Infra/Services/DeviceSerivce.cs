using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

using Microsoft.AspNetCore.Http.Features;

namespace BoxServer.Infra.Service
{
    public class DeviceService(DeviceRepository deviceRepository) 
    {
        public async Task<List<Device>> GetDevices()
        {
            return await deviceRepository.FindMany();
        }
        public async Task<Device?> Ping(Device device)
        {
            Device? record = await deviceRepository.FindByID(device.Id);
            if (record == null)
            {
                return await deviceRepository.Create(device);    
            }
            return await deviceRepository.Update(device.Id,device);
        }
    }
}
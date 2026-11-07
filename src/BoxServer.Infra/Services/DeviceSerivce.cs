using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

namespace BoxServer.Infra.Service
{
    public class DeviceService(DeviceRepository deviceRepository) 
    {
        public async Task<List<Device>> GetDevices()
        {
            return await deviceRepository.FindMany();
        }
        public async Task<Device> Ping(Device device)
        {
            return await deviceRepository.Create(device);
        }
    }
}
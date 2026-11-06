using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;

namespace BoxServer.Infra.Repository;
 
public class DeviceRepository(DBContext _context) : BaseRepository<Device>(_context)
{
}
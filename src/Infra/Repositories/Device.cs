using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;

namespace Zin.Infra.Repository;
 
public class DeviceRepository(DBContext _context) : BaseRepository<Device>(_context)
{
}
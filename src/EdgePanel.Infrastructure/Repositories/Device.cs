using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;

namespace EdgePanel.Infrastructure.Repository;
 
public class DeviceRepository(DBContext _context) : BaseRepository<Device>(_context)
{
}
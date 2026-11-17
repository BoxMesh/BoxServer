using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;

namespace BoxServer.Infra.Repository;

public class EnrollmentTokenRepository(DBContext _context) : BaseRepository<EnrollmentToken>(_context)
{
}

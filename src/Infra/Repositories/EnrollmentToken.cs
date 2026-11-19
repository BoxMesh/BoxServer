using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;

namespace Zin.Infra.Repository;

public class EnrollmentTokenRepository(DBContext _context) : BaseRepository<EnrollmentToken>(_context)
{
}

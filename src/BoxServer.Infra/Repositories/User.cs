using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;

namespace BoxServer.Infra.Repository;
 
public class UserRepository(DBContext _context) : BaseRepository<User>(_context)
{
    public async Task<User?> FindByEmail(string Email)
    {
        return await FindBy("Email",Email);
    }
}
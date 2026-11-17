using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace BoxServer.Infra.Repository;
 
public class UserRepository(DBContext _context) : BaseRepository<User>(_context)
{
    public async Task<User?> FindByEmail(string Email)
    {
        return await FindBy("Email",Email);
    }

    /// <summary>True when another account (not exceptId) uses this email, ignoring case.</summary>
    public async Task<bool> EmailTaken(string email, Guid? exceptId = null)
    {
        string lower = email.ToLower();
        return await _context.Set<User>().AnyAsync(u => u.Email.ToLower() == lower && u.Id != exceptId);
    }
}
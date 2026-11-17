using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace BoxServer.Infra.Repository;

public class GroupRepository(DBContext _context) : BaseRepository<Group>(_context)
{
    /// <summary>True when another group (not <paramref name="exceptId"/>) already has this name, ignoring case.</summary>
    public async Task<bool> NameTaken(string name, Guid? exceptId = null)
    {
        string lower = name.ToLower();
        return await _context.Set<Group>().AnyAsync(g => g.Name.ToLower() == lower && g.Id != exceptId);
    }
}

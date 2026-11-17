using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

namespace BoxServer.Infra.Service;

public class GroupService(GroupRepository groupRepository)
{
    public async Task<List<Group>> GetGroups()
    {
        List<Group> groups = await groupRepository.FindMany();
        return groups.OrderBy(g => g.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public async Task<Group?> GetById(Guid id) => await groupRepository.FindByID(id);

    /// <summary>Creates the group; null when the name is already taken.</summary>
    public async Task<Group?> Create(string name)
    {
        if (await groupRepository.NameTaken(name))
        {
            return null;
        }
        return await groupRepository.Create(new Group { Id = Guid.NewGuid(), Name = name });
    }

    /// <summary>Renames the group. Group is null when it does not exist; Taken when another group already has the name.</summary>
    public async Task<(Group? Group, bool Taken)> Rename(Guid id, string name)
    {
        Group? record = await groupRepository.FindByID(id);
        if (record == null)
        {
            return (null, false);
        }
        if (await groupRepository.NameTaken(name, id))
        {
            return (record, true);
        }
        record.Name = name;
        record.UpdatedAt = DateTime.UtcNow;
        return (await groupRepository.Update(id, record), false);
    }

    public async Task<Group?> Delete(Guid id) => await groupRepository.Delete(id);
}

using EdgePanel.Infrastructure.Database.Entities;

namespace EdgePanel.Infrastructure.Service.Base;

public abstract class IService<Entity>
    where Entity : IBase
{
    public abstract Task<Entity> CreateAsync(Entity entity);

    public abstract Task<List<Entity>> GetAllAsync();

    public abstract Task<Entity?> GetByIdAsync(Guid id);

    public abstract Task<bool> UpdateAsync(Guid id, Entity entity);

    public abstract Task<bool> DeleteAsync(Guid id);
}
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service.Base;
namespace EdgePanel.Infrastructure.Service;

public class AuthService : IService<User>
{
    public override Task<User> CreateAsync(User entity)
    {
        return Task.FromResult(entity);
    }

    public override async Task<bool> DeleteAsync(Guid id)
    {
        return await Task.FromResult(false);
    }

    public override Task<List<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> UpdateAsync(Guid id, User entity)
    {
        throw new NotImplementedException();
    }
}
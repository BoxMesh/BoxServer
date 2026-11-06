
using BoxServer.Infra.Database.Entities;

public interface IBaseRepository<T> where T : IBase
{
    Task<T> Create(T Entity);
    Task<T?> FindBy(string By, object? Value);
    Task<T?> Update(Guid ID, T Entity);
    Task<T?> Delete(Guid ID);
}
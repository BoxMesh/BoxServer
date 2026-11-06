using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;

public class BaseRepository<T>(DBContext _context) : IBaseRepository<T> where T : IBase
{
    private readonly DbSet<T> _dbSet = _context.Set<T>();

    public async Task<T> Create(T Entity)
    {
        await _dbSet.AddAsync(Entity);
        await _context.SaveChangesAsync();
        return Entity;
    }
    public async Task<T?> FindBy(string By, object? Value)
    {
        PropertyInfo? propertyInfo = typeof(T).GetProperty(By);
        if (propertyInfo == null)
        {
            return null;
        }
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyInfo);
        var constant = Expression.Constant(Value, propertyInfo.PropertyType);
        var condition = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
        return await _dbSet.FirstOrDefaultAsync(lambda);
    }

    public async Task<T?> FindByID(Guid ID)
    {
        return await _dbSet.FirstOrDefaultAsync((e) => e.Id == ID);
    }
    public async Task<T?> Update(Guid ID, T Entity)
    {
        T? record = await FindBy("ID", ID);
        if (record == null) 
        {
            return null;
        }
        _context.Entry(record).CurrentValues.SetValues(Entity);
        await _context.SaveChangesAsync();
        return record;
    }
    public async Task<T?> Delete(Guid ID)
    {
        T? record = await FindBy("ID", ID);
        if (record == null)
        {
            return null;
        }
        _dbSet.Remove(record);
        await _context.SaveChangesAsync();
        return record;
    }
}

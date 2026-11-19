using Zin.Infra.Database.Configurations;

using Microsoft.EntityFrameworkCore;

namespace Zin.Infra.Database.Context;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UserConfiguration).Assembly
        );
    }
}
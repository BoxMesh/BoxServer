using EdgePanel.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;

namespace EdgePanel.Infrastructure.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Entities.User> Users { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UserConfiguration).Assembly
        );
    }
}
using BoxServer.Infra.Database.Context;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoxServer.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DBContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
        );
        services.AddScoped<PasswordHasher<User>>();
        
        services.AddScoped<UserService>();
        services.AddScoped<UserRepository>();

        services.AddScoped<DeviceService>();
        services.AddScoped<DeviceRepository>();
        return services;
    }

}
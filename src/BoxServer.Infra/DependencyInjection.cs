using Zin.Infra.Database.Context;
using Zin.Infra.Database.Entities;
using Zin.Infra.Repository;
using Zin.Infra.Service;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zin.Infra;

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
        services.AddScoped<DeviceMetricRepository>();

        services.AddScoped<AccessService>();
        services.AddScoped<AccessRepository>();

        services.AddScoped<GroupService>();
        services.AddScoped<GroupRepository>();
        return services;
    }

}
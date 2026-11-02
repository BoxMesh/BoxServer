using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Services;
using EdgePanel.Infrastructure.Services.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EdgePanel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSQL")
            )
        );

        services.AddScoped<IService<Device>, DeviceService>();

        return services;
    }

}
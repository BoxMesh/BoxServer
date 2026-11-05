using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service;
using EdgePanel.Infrastructure.Service.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

namespace EdgePanel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(new NpgsqlConnectionStringBuilder
            {
                Host = configuration["POSTGRES_HOST"],
                Port = int.Parse(configuration["POSTGRES_PORT"] ?? "5432"),
                Database = configuration["POSTGRES_DB"],
                Username = configuration["POSTGRES_USER"],
                Password = configuration["POSTGRES_PASSWORD"],
            }.ConnectionString)
        );
        services.AddScoped<AuthService>();

        return services;
    }

}
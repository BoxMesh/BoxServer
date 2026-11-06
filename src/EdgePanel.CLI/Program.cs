using System.CommandLine;
using EdgePanel.CLI.Subcommand.Admin;
using DotNetEnv;
using EdgePanel.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EdgePanel.Infrastructure.Service;

public partial class Program
{
    private static async Task<int> Main(string[] args)
    {
        Env.Load();
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        ServiceCollection services = new ServiceCollection();
        services.AddInfrastructure(configuration);
        await using var provider = services.BuildServiceProvider();
        RootCommand root = new("BoxServer CLI");

        root.Subcommands.Add(Admin.command(provider.GetRequiredService<UserService>()));
        return await root.Parse(args).InvokeAsync();
    }
}

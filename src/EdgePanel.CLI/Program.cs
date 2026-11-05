using System.CommandLine;

using EdgePanel.CLI.Subcommand.Admin;
using EdgePanel.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EdgePanel.CLI;

public partial class Program
{
    private static async Task<int> Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvFile()
            .AddEnvironmentVariables()
            .Build();
        ServiceProvider services = new ServiceCollection()
            .AddInfrastructure(configuration)
            .BuildServiceProvider();

        RootCommand root = new("BoxServer CLI");
        root.Subcommands.Add(Admin.cmd(services));
        return await root.Parse(args).InvokeAsync();
    }
}

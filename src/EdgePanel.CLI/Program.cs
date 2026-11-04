using System.CommandLine;
using EdgePanel.CLI.Subcommand.Admin;
using EdgePanel.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace EdgePanel.CLI;

public partial class Program
{
    private static async Task<int> Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = AppContext.BaseDirectory
        });
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
        builder.Services.AddInfrastructure(builder.Configuration);
        using IHost host = builder.Build();

        RootCommand root = new("EdgePanel CLI");
        root.Subcommands.Add(Admin.cmd(host.Services));
        return await root.Parse(args).InvokeAsync();
    }
}
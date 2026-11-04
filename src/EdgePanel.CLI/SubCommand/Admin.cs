using System.CommandLine;

using EdgePanel.Infrastructure.Service;

using Microsoft.Extensions.DependencyInjection;
namespace EdgePanel.CLI.Subcommand.Admin
{
    public static class Admin
    {
        public static Command cmd(IServiceProvider services)
        {
            Command Admin = new("create-admin", "Tạo tài khoản admin");
            Option<String> email = new("--email")
            {
                Description = "Email",
                Required = true,
            };
            Option<String> password = new("--password")
            {
                Description = "Password",
                Required = true,
            };
            Admin.Options.Add(email);
            Admin.Options.Add(password);
            Admin.SetAction(
                async (parseResult, cancellationToken) =>
                {
                    string Email = parseResult.GetValue(email)!;
                    string Password = parseResult.GetValue(password)!;
                    using IServiceScope scope = services.CreateScope();
                    AuthService authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                    try
                    {
                        if (await authService.SignUp(Email, Password))
                        {
                            Console.WriteLine("Registration successful.");
                            return;
                        }
                        Console.WriteLine("Registration failed.");
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                    {
                        Console.WriteLine("Email already exists.");
                    }
                    
                }
            );

            return Admin;
        }

    }

}
using System.CommandLine;

using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Repository;
namespace EdgePanel.CLI.Subcommand.Admin
{
    public class Admin
    {
        public static Command command(UserRepository _userRepository)
        {
            Command command = new("create-admin", "Tạo tài khoản admin");
            Option<string> email = new("--email")
            {
                Description = "Email",
                Required = true,
            };
            Option<string> password = new("--password")
            {
                Description = "Password",
                Required = true,
            };
            command.Options.Add(email);
            command.Options.Add(password);
            command.SetAction(
                async (parseResult, cancellationToken) =>
                {
                    string Email = parseResult.GetValue(email)!;
                    string Password = parseResult.GetValue(password)!;
                    try
                    {
                        await _userRepository.Create(new User
                        {
                            Email = Email,
                            Password = Password, 
                        });
                    } catch(Microsoft.EntityFrameworkCore.DbUpdateException){
                        Console.WriteLine("Failed to create user.");
                    }
                }
            );
            return command;
        }

    }

}
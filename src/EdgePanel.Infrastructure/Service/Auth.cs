using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace EdgePanel.Infrastructure.Service;

public class AuthService(AppDbContext context) : IService<User>
{   
    private readonly PasswordHasher<User> hasher = new();
    public async Task<bool> SignUp(string Email, string Password)
    {
        var user = new User
        {
            Email = Email
        };
        user.Password = hasher.HashPassword(user, Password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<User?> SignIn(String Email, String Password)
    {
        User? user = await context.Users.FirstOrDefaultAsync(user => user.Email == Email);
        if (user == null)
        {
            return null;
        }
        if (hasher.VerifyHashedPassword(
            user!,
            user!.Password,
            Password
        ) == PasswordVerificationResult.Failed)
        {
           return null;
        }
        return user;
    }
}
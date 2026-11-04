using EdgePanel.Infrastructure.Database.Context;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service.Base;

using Microsoft.EntityFrameworkCore;
namespace EdgePanel.Infrastructure.Service;

public class AuthService(AppDbContext context) : IService<User>
{
    public async Task<bool> SignUp(String Email, String Password)
    {
        await context.Users.AddAsync(new User
        {
            Email = Email,
            Password = Password,
        });
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> SignIn(String Email)
    {
        User? user = await context.Users.FirstOrDefaultAsync(user => user.Email == Email);
        return user != null;
    }
}
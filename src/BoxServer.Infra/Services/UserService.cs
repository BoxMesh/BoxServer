using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

using Microsoft.AspNetCore.Identity;
namespace BoxServer.Infra.Service
{
    public class UserService(
        UserRepository userRepository,
        PasswordHasher<User> hasher
    )
    {
        public async Task<User> SignUp(string Email, string Password)
        {
            User user = new User
            {
                Email = Email,
            };
            user.Password = hasher.HashPassword(user,Password);
            return await userRepository.Create(user);
        }

        public async Task<User?> SignIn(string Email, string Password)
        {
            User? user = await userRepository.FindByEmail(Email);
            if (user == null)
            {
                return null;
            }
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user,user.Password,Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return user;
        }
    }
}
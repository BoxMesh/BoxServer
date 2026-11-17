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
        /// <summary>Creates a System account; the only way to make an account today.</summary>
        public async Task<User> SignUp(string Email, string Password)
        {
            User user = new User
            {
                Email = Email,
                Role = Roles.System,
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
            if (result == PasswordVerificationResult.Failed || user.Disabled)
            {
                return null;
            }
            user.LastSignIn = DateTime.UtcNow;
            return await userRepository.Update(user.Id, user);
        }

        public bool PasswordMatches(User user, string password) =>
            hasher.VerifyHashedPassword(user, user.Password, password) != PasswordVerificationResult.Failed;

        public async Task<List<User>> GetUsers()
        {
            List<User> users = await userRepository.FindMany();
            return users.OrderBy(u => u.CreatedAt).ToList();
        }

        public async Task<User?> GetById(Guid id) => await userRepository.FindByID(id);

        /// <summary>Creates the account; null when the email is already used.</summary>
        public async Task<User?> Create(string name, string email, string password, string role)
        {
            if (await userRepository.EmailTaken(email))
            {
                return null;
            }
            User user = new User { Name = name, Email = email, Role = role };
            user.Password = hasher.HashPassword(user, password);
            return await userRepository.Create(user);
        }

        /// <summary>Changes only what is given; null when the account does not exist.</summary>
        public async Task<User?> Update(Guid id, string? name, bool? disabled, string? password)
        {
            User? user = await userRepository.FindByID(id);
            if (user == null)
            {
                return null;
            }
            user.Name = name ?? user.Name;
            user.Disabled = disabled ?? user.Disabled;
            if (password != null)
            {
                user.Password = hasher.HashPassword(user, password);
            }
            user.UpdatedAt = DateTime.UtcNow;
            return await userRepository.Update(id, user);
        }

        public async Task<User?> Delete(Guid id) => await userRepository.Delete(id);
    }
}
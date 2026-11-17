namespace BoxServer.Infra.Database.Entities;

public static class Roles
{
    public const string System = "System";
    public const string User = "User";
}

public class User : IBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    /// <summary>A disabled account cannot sign in.</summary>
    public bool Disabled { get; set; } = false;
    public DateTime? LastSignIn { get; set; } = null;
    /// <summary>Roles.System or Roles.User; accounts made without a role are Users.</summary>
    public string Role { get; set; } = Roles.User;
}
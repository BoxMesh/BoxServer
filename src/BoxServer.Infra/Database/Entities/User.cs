namespace Zin.Infra.Database.Entities;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Operator = "Operator";
}

public class User : IBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    /// <summary>A disabled account cannot sign in.</summary>
    public bool Disabled { get; set; } = false;
    public DateTime? LastSignIn { get; set; } = null;
    /// <summary>Roles.Admin or Roles.Operator; accounts made without a role can only look.</summary>
    public string Role { get; set; } = Roles.Operator;
}
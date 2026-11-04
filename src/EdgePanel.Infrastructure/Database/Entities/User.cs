namespace EdgePanel.Infrastructure.Database.Entities;

public class User : IBase
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
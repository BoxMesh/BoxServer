namespace EdgePanel.API.DTO.Auth;

public record SignInRequest(string Email, string Password);
public record SignOutRequest(string RefreshToken);
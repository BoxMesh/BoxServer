namespace Zin.API.DTO.Auth;

public record SignInRequest(string Email, string Password);
public record SignInResponse(string AccessToken, string RefreshToken);
/// <summary>The account BoxMesh keeps after log-in; Role is System or User.</summary>
public record AccountResponse(Guid Id, string Email, string Role);
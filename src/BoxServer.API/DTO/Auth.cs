namespace BoxServer.API.DTO.Auth;

public record SignInRequest(string Email, string Password);
public record SignInResponse(string AccessToken, string RefreshToken);
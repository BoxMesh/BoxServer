using Zin.API.DTO.Auth;
using Zin.API.Secret;
using Zin.Infra.Database.Entities;
using Zin.Infra.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class AuthController(
    UserService userService,
    JWTSecret jwtSecret
) : ControllerBase
{
    /// <summary>For API clients: returns the tokens to send as a Bearer header.</summary>
    [HttpPost("/sign-in")]
    public async Task<ActionResult<SignInResponse>> signIn([FromBody] SignInRequest signInRequest)
    {
        User? user = await userService.SignIn(signInRequest.Email,signInRequest.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(new SignInResponse(
            jwtSecret.GenerateAccessToken(user),
            jwtSecret.GenerateRefreshToken(user)
        ));
    }

    /// <summary>For the web console: sets the session cookie and returns the account.</summary>
    [HttpPost("/log-in")]
    public async Task<ActionResult<AccountResponse>> LogIn([FromBody] SignInRequest request)
    {
        User? user = await userService.SignIn(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        jwtSecret.SetSessionCookie(Response, user);
        return Ok(new AccountResponse(user.Id, user.Email, user.Role, user.Name));
    }

    [HttpPost("/log-out")]
    public IActionResult LogOut()
    {
        JWTSecret.ClearSessionCookie(Response);
        return NoContent();
    }
}

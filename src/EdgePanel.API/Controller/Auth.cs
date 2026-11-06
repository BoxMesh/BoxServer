using EdgePanel.API.DTO.Auth;
using EdgePanel.API.Secret;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class AuthController(
    UserService userService,
    JWTSecret jwtSecret
) : ControllerBase
{
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
            jwtSecret.GenerateAccessToken(user)
        ));
    }
}
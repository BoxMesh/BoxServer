using BoxServer.API.DTO.Auth;
using BoxServer.API.Secret;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;
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
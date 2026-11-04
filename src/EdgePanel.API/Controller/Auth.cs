using EdgePanel.API.DTO.Auth;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service;

using Microsoft.AspNetCore.Mvc;
namespace EdgePanel.API.Controller;


[ApiController]
[Route("[controller]")]
public class AuthController(AuthService _authService) : ControllerBase
{
    [HttpPost("/sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest SignIn)
    {
        if (await _authService.SignIn(SignIn.Email))
        {
            return Ok(SignIn);
        }
        return Unauthorized("Unauthorized");
    }
}
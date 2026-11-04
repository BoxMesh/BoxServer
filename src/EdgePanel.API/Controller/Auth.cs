using EdgePanel.API.DTO.Auth;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service;

using Microsoft.AspNetCore.Mvc;
namespace EdgePanel.API.Controller;


[ApiController]
[Route("[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        User user = await authService.CreateAsync(new User
        {
            Email = request.Email,
            Password = request.Password
        });
        return Ok(new { Message = "Sign-in successful", Email = user.Email });
    }
}
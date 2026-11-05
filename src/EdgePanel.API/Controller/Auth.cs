using System.Security.Claims;
using System.Text;
using EdgePanel.API.DTO.Auth;
using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
namespace EdgePanel.API.Controller;


[ApiController]
[Route("[controller]")]
public class AuthController(AuthService _authService,IConfiguration configuration) : ControllerBase
{
    private string CreateToken(User user, string keyName, TimeSpan lifetime)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration[keyName]!));
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = "BoxServer",
            Audience = "BoxServer",
            Expires = DateTime.UtcNow.Add(lifetime),
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        };
        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
    private void SetTokenCookie(User user, string cookieName, string cookieValue, TimeSpan lifetime)
    {
        Response.Cookies.Append(cookieName, cookieValue, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.Add(lifetime),
        });
    }

    [HttpPost("/log-in")]
    public async Task<IActionResult> LogIn([FromBody] SignInRequest SignIn)
    {
        User? user = await _authService.SignIn(SignIn.Email, SignIn.Password);
        if (user == null)
        {
            return Unauthorized("Unauthorized");
        }
        SetTokenCookie(
            user, 
            "AccessToken",
            CreateToken(user, "AccessKey", TimeSpan.FromMinutes(15)), 
            TimeSpan.FromMinutes(15)
        );
        SetTokenCookie(
            user, 
            "RefreshToken", 
            CreateToken(user, "SecretKey", TimeSpan.FromDays(30)), 
            TimeSpan.FromDays(30)
        );
        return Ok(new { user.Id, user.Email });
    }

    [HttpPost("/log-out")]
    public async Task<IActionResult> LogOut()
    {
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");
        return Ok();
    }
    

}
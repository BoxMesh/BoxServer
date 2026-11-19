using System.Text;

using Zin.Infra.Database.Entities;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Zin.API.Secret;

public class JWTSecret(IConfiguration configuration)
{
    public const string SessionCookie = "session";
    public const string RoleClaim = "role";
    private static readonly TimeSpan SessionLifetime = TimeSpan.FromHours(8);

    private SigningCredentials GetSigningCredentials()
    {
        string secretKey = configuration["Jwt:SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }
    public IDictionary<string,object> GenerateClaims(User user)
    {
        Dictionary<string, object> claims = new Dictionary<string, object>();
        claims.Add("id", user.Id);
        claims.Add("email", user.Email);
        claims.Add(RoleClaim, user.Role);
        return claims;
    }
    private string CreateToken(User user, TimeSpan lifetime)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Claims = GenerateClaims(user),
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = GetSigningCredentials(),
        });
    }
    public string GenerateAccessToken(User user) => CreateToken(user, TimeSpan.FromMinutes(15));
    public string GenerateRefreshToken(User user) => CreateToken(user, TimeSpan.FromDays(7));

    public void SetSessionCookie(HttpResponse response, User user)
    {
        response.Cookies.Append(SessionCookie, CreateToken(user, SessionLifetime), new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = SessionLifetime,
            Secure = response.HttpContext.Request.IsHttps,
        });
    }

    public static void ClearSessionCookie(HttpResponse response)
    {
        response.Cookies.Delete(SessionCookie, new CookieOptions { Path = "/" });
    }
}

using System.Text;

using BoxServer.Infra.Database.Entities;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BoxServer.API.Secret;

public class JWTSecret(IConfiguration configuration)
{
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
        return claims;
    }
    public string GenerateAccessToken(User user)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Claims = GenerateClaims(user),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = GetSigningCredentials(),
        });
    }
    public string GenerateRefreshToken(User user)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Claims = GenerateClaims(user),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = GetSigningCredentials(),
        });
    }
}
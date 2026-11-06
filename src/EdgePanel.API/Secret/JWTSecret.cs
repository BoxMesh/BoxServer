using System.Text;

using EdgePanel.Infrastructure.Database.Entities;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EdgePanel.API.Secret;

public class JWTSecret(IConfiguration configuration)
{
    private SigningCredentials GetSigningCredentials()
    {
        string secretKey = configuration["JWT_SecretKey"]!;
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
            Issuer = configuration["JWT_ValidIssuer"],
            Audience = configuration["JWT_ValidAudience"],
            Claims = GenerateClaims(user),
            Expires = DateTime.Now.AddMinutes(15),
            SigningCredentials = GetSigningCredentials(),
        });
    }
    public string GenerateRefreshToken(User user)
    {
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT_ValidIssuer"],
            Audience = configuration["JWT_ValidAudience"],
            Claims = GenerateClaims(user),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = GetSigningCredentials(),
        });
    }
}
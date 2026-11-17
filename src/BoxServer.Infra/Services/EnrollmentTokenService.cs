using System.Security.Cryptography;

using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;

namespace BoxServer.Infra.Service;

public class EnrollmentTokenService(EnrollmentTokenRepository tokenRepository)
{
    public async Task<List<EnrollmentToken>> GetTokens()
    {
        List<EnrollmentToken> tokens = await tokenRepository.FindMany();
        return tokens.OrderByDescending(t => t.CreatedAt).ToList();
    }

    public async Task<EnrollmentToken?> GetById(Guid id) => await tokenRepository.FindByID(id);

    /// <summary>Creates a token valid for <paramref name="lifetime"/>.</summary>
    public async Task<EnrollmentToken> Create(TimeSpan lifetime, int? maxDevices, Guid? createdBy) =>
        await tokenRepository.Create(new EnrollmentToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToHexStringLower(RandomNumberGenerator.GetBytes(32)),
            ExpiresAt = DateTime.UtcNow + lifetime,
            MaxDevices = maxDevices,
            CreatedBy = createdBy,
        });

    /// <summary>Revokes the token: it is deleted, so an agent presenting it is no longer let in.</summary>
    public async Task<EnrollmentToken?> Delete(Guid id) => await tokenRepository.Delete(id);
}

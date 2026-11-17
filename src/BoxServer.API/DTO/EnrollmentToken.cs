using BoxServer.Infra.Database.Entities;

namespace BoxServer.API.DTO.EnrollmentTokens;

/// <summary>POST /enrollment-tokens. Lifetime is 1h, 24h or 7d; MaxDevices is 1 to 1,000,000; leave it out for no limit.</summary>
public record CreateEnrollmentTokenRequest(string? Lifetime, int? MaxDevices);

/// <summary>A token in the shape BoxMesh reads (EnrollmentToken in BoxMesh/src/types/admin.ts).</summary>
public record EnrollmentTokenResponse(Guid Id, string Token, DateTime ExpiresAt, int? MaxDevices)
{
    public static EnrollmentTokenResponse From(EnrollmentToken t) => new(t.Id, t.Token, t.ExpiresAt, t.MaxDevices);
}

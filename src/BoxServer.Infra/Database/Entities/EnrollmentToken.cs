namespace BoxServer.Infra.Database.Entities;

/// <summary>
/// What an agent presents to enroll. It only authenticates the enrollment; it belongs to no group. CreatedBy records the
/// issuing System account. The token is kept in full so the System account can copy it again later.
/// </summary>
public class EnrollmentToken : IBase
{
    /// <summary>64 lowercase hex characters.</summary>
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    /// <summary>How many devices may enroll with this token; null is no limit.</summary>
    public int? MaxDevices { get; set; }
}

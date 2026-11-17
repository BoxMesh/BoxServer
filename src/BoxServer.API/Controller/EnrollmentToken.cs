using BoxServer.API.DTO.EnrollmentTokens;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Only a System account issues, reads and revokes enrollment tokens. A token only authenticates an enrollment and has no other settings.
[ApiController]
[Authorize(Roles = Roles.System)]
[Route("/enrollment-tokens")]
public class EnrollmentTokenController(EnrollmentTokenService tokenService) : ControllerBase
{
    private static readonly Dictionary<string, TimeSpan> Lifetimes = new()
    {
        ["1h"] = TimeSpan.FromHours(1),
        ["24h"] = TimeSpan.FromHours(24),
        ["7d"] = TimeSpan.FromDays(7),
    };

    [HttpGet]
    public async Task<ActionResult<List<EnrollmentTokenResponse>>> GetTokens()
    {
        List<EnrollmentToken> tokens = await tokenService.GetTokens();
        return Ok(tokens.Select(t => EnrollmentTokenResponse.From(t)).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EnrollmentTokenResponse>> GetToken(Guid id)
    {
        EnrollmentToken? token = await tokenService.GetById(id);
        return token == null ? NotFound() : Ok(EnrollmentTokenResponse.From(token));
    }

    /// <summary>Issues a token.</summary>
    [HttpPost]
    public async Task<ActionResult<EnrollmentTokenResponse>> CreateToken([FromBody] CreateEnrollmentTokenRequest request)
    {
        if (!Lifetimes.TryGetValue(request.Lifetime ?? "", out TimeSpan lifetime))
        {
            return BadRequest("lifetime must be 1h, 24h or 7d");
        }
        if (request.MaxDevices is < 1 or > 1_000_000)
        {
            return BadRequest("maxDevices must be 1 to 1000000, or left out for no limit");
        }
        Guid? me = Guid.TryParse(User.FindFirst("id")?.Value, out Guid id) ? id : null;
        EnrollmentToken record = await tokenService.Create(lifetime, request.MaxDevices, me);
        return Ok(EnrollmentTokenResponse.From(record));
    }

    /// <summary>Revokes the token.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteToken(Guid id)
    {
        return await tokenService.Delete(id) == null ? NotFound() : NoContent();
    }
}

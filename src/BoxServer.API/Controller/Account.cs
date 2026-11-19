using Zin.API.DTO.Users;
using Zin.Infra.Database.Entities;
using Zin.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// The signed-in account looking after itself: any role, only its own name and password.
[ApiController]
[Authorize]
[Route("/account")]
public class AccountController(UserService userService, AccessService accessService) : ControllerBase
{
    /// <summary>Changes the caller's own name and/or password; a new password needs the current one.</summary>
    [HttpPatch]
    public async Task<ActionResult<UserResponse>> UpdateAccount([FromBody] UpdateAccountRequest request)
    {
        if (!Guid.TryParse(User.FindFirst("id")?.Value, out Guid me) || await userService.GetById(me) is not User user)
        {
            return Unauthorized();
        }
        string? name = request.Name?.Trim();
        string? error = name == null ? null : AccountRules.NameError(name);
        if (error == null && request.Password != null)
        {
            error = AccountRules.PasswordError(request.Password)
                ?? (userService.PasswordMatches(user, request.CurrentPassword ?? "") ? null : "current password is wrong");
        }
        if (error != null)
        {
            return BadRequest(error);
        }
        User? updated = await userService.Update(me, name, null, request.Password);
        return updated == null ? Unauthorized() : Ok(UserResponse.From(updated, await accessService.AccessOf(me)));
    }
}

using BoxServer.API.DTO.Users;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Only a System account manages accounts. An account edits itself through PATCH /account.
[ApiController]
[Authorize(Roles = Roles.System)]
[Route("/users")]
public class UserController(UserService userService, AccessService accessService) : ControllerBase
{
    // True when id is the account making this request.
    private bool IsSelf(Guid id) => Guid.TryParse(User.FindFirst("id")?.Value, out Guid me) && me == id;

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        List<User> users = await userService.GetUsers();
        var access = await accessService.AccessByUser();
        return Ok(users.Select(u => UserResponse.From(u, access.GetValueOrDefault(u.Id))).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id)
    {
        User? user = await userService.GetById(id);
        return user == null ? NotFound() : Ok(UserResponse.From(user, await accessService.AccessOf(id)));
    }

    /// <summary>Creates the account outright: the System account sets the password and no email is sent. Access is granted afterwards.</summary>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        string name = (request.Name ?? "").Trim();
        string email = (request.Email ?? "").Trim();
        string? error = AccountRules.NameError(name) ?? AccountRules.EmailError(email) ?? AccountRules.PasswordError(request.Password);
        if (error == null && (request.Role ?? Roles.User) != Roles.User)
        {
            error = "role must be User: there is one System account";
        }
        if (error != null)
        {
            return BadRequest(error);
        }
        User? user = await userService.Create(name, email, request.Password!, Roles.User);
        return user == null ? Conflict("an account with this email already exists") : Ok(UserResponse.From(user, null));
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        string? name = request.Name?.Trim();
        string? error = name == null ? null : AccountRules.NameError(name);
        if (error == null && request.Password != null)
        {
            error = AccountRules.PasswordError(request.Password);
        }
        if (error == null && request.Disabled == true && IsSelf(id))
        {
            error = "you cannot disable your own account";
        }
        Guid[] groupIds = request.Access?.GroupIds ?? [];
        Guid[] deviceIds = request.Access?.DeviceIds ?? [];
        if (error == null && request.Access != null && !await accessService.AllExist(groupIds, deviceIds))
        {
            error = "unknown group or device";
        }
        if (error != null)
        {
            return BadRequest(error);
        }
        User? user = await userService.Update(id, name, request.Disabled, request.Password);
        if (user == null)
        {
            return NotFound();
        }
        if (request.Access != null)
        {
            await accessService.SetAccess(id, groupIds, deviceIds);
        }
        return Ok(UserResponse.From(user, await accessService.AccessOf(id)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (IsSelf(id))
        {
            return BadRequest("you cannot delete your own account");
        }
        return await userService.Delete(id) == null ? NotFound() : NoContent();
    }
}

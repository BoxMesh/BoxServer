using BoxServer.API.DTO.Groups;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Any signed-in account can read groups; only a System account can change them.
[ApiController]
[Authorize]
[Route("/groups")]
public class GroupController(GroupService groupService) : ControllerBase
{
    private const int MaxName = 100;

    // The trimmed name, or null with the reason in error when it is empty or too long.
    private static string? ValidName(GroupRequest request, out string error)
    {
        string name = (request.Name ?? "").Trim();
        error = name.Length == 0 ? "name is required"
            : name.Length > MaxName ? $"name must be at most {MaxName} characters"
            : "";
        return error == "" ? name : null;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupResponse>>> GetGroups()
    {
        List<Group> groups = await groupService.GetGroups();
        return Ok(groups.Select(GroupResponse.From).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GroupResponse>> GetGroup(Guid id)
    {
        Group? group = await groupService.GetById(id);
        return group == null ? NotFound() : Ok(GroupResponse.From(group));
    }

    [Authorize(Roles = Roles.System)]
    [HttpPost]
    public async Task<ActionResult<GroupResponse>> CreateGroup([FromBody] GroupRequest request)
    {
        string? name = ValidName(request, out string error);
        if (name == null)
        {
            return BadRequest(error);
        }
        Group? group = await groupService.Create(name);
        return group == null ? Conflict("a group with this name already exists") : Ok(GroupResponse.From(group));
    }

    /// <summary>Renames the group.</summary>
    [Authorize(Roles = Roles.System)]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<GroupResponse>> UpdateGroup(Guid id, [FromBody] GroupRequest request)
    {
        string? name = ValidName(request, out string error);
        if (name == null)
        {
            return BadRequest(error);
        }
        (Group? group, bool taken) = await groupService.Rename(id, name);
        if (group == null)
        {
            return NotFound();
        }
        return taken ? Conflict("a group with this name already exists") : Ok(GroupResponse.From(group));
    }

    [Authorize(Roles = Roles.System)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        return await groupService.Delete(id) == null ? NotFound() : NoContent();
    }
}

using BoxServer.API.DTO.Devices;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("/devices")]
public class DeviceController(DeviceService deviceService, AccessService accessService) : ControllerBase
{
    private const int MaxAlias = 100;

    // Each range is cut into as many points as BoxMesh's Performance chart draws: 60, 96 and 168.
    private static readonly Dictionary<string, (TimeSpan Span, TimeSpan Step)> Ranges = new()
    {
        ["1h"] = (TimeSpan.FromHours(1), TimeSpan.FromMinutes(1)),
        ["24h"] = (TimeSpan.FromHours(24), TimeSpan.FromMinutes(15)),
        ["7d"] = (TimeSpan.FromDays(7), TimeSpan.FromHours(1)),
    };

    // The signed-in account's ID; an empty one matches no grant.
    private Guid Me => Guid.TryParse(User.FindFirst("id")?.Value, out Guid me) ? me : Guid.Empty;

    private async Task<bool> CanSee(Guid deviceId) => User.IsInRole(Roles.System) || await accessService.CanSee(Me, deviceId);

    // The device, or null when it does not exist or the caller may not see it.
    private async Task<Device?> VisibleDevice(Guid id) => await CanSee(id) ? await deviceService.GetById(id) : null;

    [HttpGet()]
    public async Task<ActionResult<List<DeviceResponse>>> GetDevices()
    {
        List<Device> devices = User.IsInRole(Roles.System) ? await deviceService.GetDevices() : await accessService.DevicesFor(Me);
        Dictionary<Guid, Guid[]> groups = await accessService.GroupIdsByDevice();
        return Ok(devices.Select(d => DeviceResponse.From(d, groups.GetValueOrDefault(d.Id, []))).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceResponse>> GetDevice(Guid id)
    {
        Device? device = await VisibleDevice(id);
        return device == null ? NotFound() : Ok(DeviceResponse.From(device, await accessService.GroupIdsOf(id)));
    }

    /// <summary>Sets the device's alias; an empty one clears it.</summary>
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<DeviceResponse>> UpdateDevice(Guid id, [FromBody] UpdateDeviceRequest request)
    {
        string alias = (request.Alias ?? "").Trim();
        if (alias.Length > MaxAlias)
        {
            return BadRequest($"alias must be at most {MaxAlias} characters");
        }
        if (!await CanSee(id))
        {
            return NotFound();
        }
        Device? device = await deviceService.SetAlias(id, alias);
        return device == null ? NotFound() : Ok(DeviceResponse.From(device, await accessService.GroupIdsOf(id)));
    }

    [Authorize(Roles = Roles.System)]
    [HttpPut("{id:guid}/groups")]
    public async Task<ActionResult<DeviceResponse>> SetGroups(Guid id, [FromBody] SetDeviceGroupsRequest request)
    {
        Guid[] groupIds = request.GroupIds ?? [];
        Device? device = await deviceService.GetById(id);
        if (device == null)
        {
            return NotFound();
        }
        if (!await accessService.AllExist(groupIds, []))
        {
            return BadRequest("unknown group");
        }
        await accessService.SetDeviceGroups(id, groupIds);
        return Ok(DeviceResponse.From(device, await accessService.GroupIdsOf(id)));
    }

    [HttpGet("{id:guid}/metrics")]
    public async Task<ActionResult<List<MetricPoint>>> GetMetrics(Guid id, [FromQuery] string range = "24h")
    {
        if (!Ranges.TryGetValue(range, out var r))
        {
            return BadRequest("range must be 1h, 24h or 7d");
        }
        Device? device = await VisibleDevice(id);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(await deviceService.GetMetrics(device.Id, DateTime.UtcNow - r.Span, r.Step));
    }

    [HttpGet("{id:guid}/processes")]
    public async Task<ActionResult<List<ProcessResponse>>> GetProcesses(Guid id)
    {
        Device? device = await VisibleDevice(id);
        return device == null ? NotFound() : Ok(ProcessResponse.FromJson(device.Processes));
    }
}

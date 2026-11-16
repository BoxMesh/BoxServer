using BoxServer.API.DTO.Devices;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Devices are addressed by their agent's ID: hostnames can repeat. An ID that isn't a UUID matches no route (404).
[ApiController]
[Authorize]
[Route("/devices")]
public class RPCDeviceController(DeviceService deviceService) : ControllerBase
{
    private const int MaxAlias = 100;

    [HttpGet()]
    public async Task<ActionResult<List<DeviceResponse>>> GetDevices()
    {
        List<Device> devices = await deviceService.GetDevices();
        return Ok(devices.Select(DeviceResponse.From).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceResponse>> GetDevice(Guid id)
    {
        Device? device = await deviceService.GetById(id);
        return device == null ? NotFound() : Ok(DeviceResponse.From(device));
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
        Device? device = await deviceService.SetAlias(id, alias);
        return device == null ? NotFound() : Ok(DeviceResponse.From(device));
    }

    // Each range is cut into as many points as BoxMesh's Performance chart draws: 60, 96 and 168.
    private static readonly Dictionary<string, (TimeSpan Span, TimeSpan Step)> Ranges = new()
    {
        ["1h"] = (TimeSpan.FromHours(1), TimeSpan.FromMinutes(1)),
        ["24h"] = (TimeSpan.FromHours(24), TimeSpan.FromMinutes(15)),
        ["7d"] = (TimeSpan.FromDays(7), TimeSpan.FromHours(1)),
    };

    [HttpGet("{id:guid}/metrics")]
    public async Task<ActionResult<List<MetricPoint>>> GetMetrics(Guid id, [FromQuery] string range = "24h")
    {
        if (!Ranges.TryGetValue(range, out var r))
        {
            return BadRequest("range must be 1h, 24h or 7d");
        }
        Device? device = await deviceService.GetById(id);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(await deviceService.GetMetrics(device.Id, DateTime.UtcNow - r.Span, r.Step));
    }

    /// <summary>The busiest processes at the agent's latest report.</summary>
    [HttpGet("{id:guid}/processes")]
    public async Task<ActionResult<List<ProcessResponse>>> GetProcesses(Guid id)
    {
        Device? device = await deviceService.GetById(id);
        return device == null ? NotFound() : Ok(ProcessResponse.FromJson(device.Processes));
    }
}

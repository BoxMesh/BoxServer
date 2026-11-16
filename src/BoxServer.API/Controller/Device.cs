using BoxServer.API.DTO.Devices;
using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Repository;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("/devices")]
public class RPCDeviceController(DeviceService deviceService) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<List<DeviceResponse>>> GetDevices()
    {
        List<Device> devices = await deviceService.GetDevices();
        return Ok(devices.Select(DeviceResponse.From).ToList());
    }

    [HttpGet("{host}")]
    public async Task<ActionResult<DeviceResponse>> GetDevice(string host)
    {
        Device? device = await deviceService.GetByHostname(host);
        return device == null ? NotFound() : Ok(DeviceResponse.From(device));
    }

    // Each range is cut into as many points as BoxMesh's Performance chart draws: 60, 96 and 168.
    private static readonly Dictionary<string, (TimeSpan Span, TimeSpan Step)> Ranges = new()
    {
        ["1h"] = (TimeSpan.FromHours(1), TimeSpan.FromMinutes(1)),
        ["24h"] = (TimeSpan.FromHours(24), TimeSpan.FromMinutes(15)),
        ["7d"] = (TimeSpan.FromDays(7), TimeSpan.FromHours(1)),
    };

    [HttpGet("{host}/metrics")]
    public async Task<ActionResult<List<MetricPoint>>> GetMetrics(string host, [FromQuery] string range = "24h")
    {
        if (!Ranges.TryGetValue(range, out var r))
        {
            return BadRequest("range must be 1h, 24h or 7d");
        }
        Device? device = await deviceService.GetByHostname(host);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(await deviceService.GetMetrics(device.Id, DateTime.UtcNow - r.Span, r.Step));
    }
}

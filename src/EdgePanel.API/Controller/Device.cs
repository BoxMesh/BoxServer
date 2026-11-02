using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Services.Base;

using Microsoft.AspNetCore.Mvc;
namespace EdgePanel.API.Controller;


[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IService<Device>? _dService;

    [HttpGet("/{id}")]
    public ActionResult<List<Device>> GetDevice(Guid id)
    {
        var device = _dService!.GetByIdAsync(id);

        if (device is null)
            return NotFound();

        return Ok(device);
    }
    
    [HttpPost("")]
    public async Task<ActionResult<Device>> CreateDevice()
    {
        var device = new Device
        {
            HostName = "Localhost",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Device newDevice = await _dService!.CreateAsync(device);
        return Ok(newDevice);
    }
}
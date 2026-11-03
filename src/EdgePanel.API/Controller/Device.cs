using EdgePanel.Infrastructure.Database.Entities;
using EdgePanel.Infrastructure.Services.Base;

using Microsoft.AspNetCore.Mvc;
namespace EdgePanel.API.Controller;


[ApiController]
[Route("[controller]")]
public class DeviceController(IService<Device> dService) : ControllerBase
{
    [HttpGet("/{id}")]
    public async Task<ActionResult<List<Device>>> GetDevice(Guid id)
    {
        var device = await dService.GetByIdAsync(id);

        if (device is null)
            return NotFound();

        return Ok(device);
    }
}
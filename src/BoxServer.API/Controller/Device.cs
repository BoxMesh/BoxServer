using BoxServer.Infra.Database.Entities;
using BoxServer.Infra.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("/device")]
public class RPCDeviceController(DeviceService deviceService) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetDevice()
    {
        List<Device> a = await deviceService.GetDevices();
        return Ok(a);
    }
}
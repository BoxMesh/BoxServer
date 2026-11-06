using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("/device")]
public class DeviceController : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetDevice()
    {
        return Ok("123");
    }
}
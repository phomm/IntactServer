using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Keep only essential custom endpoints that don't duplicate Identity functionality
    [HttpGet("config")]
    public IActionResult GetConfig()
    {
        return Ok(new
        {
            requireEmailConfirmation = true,
            message = "Email confirmation is enabled for new registrations"
        });
    }
}
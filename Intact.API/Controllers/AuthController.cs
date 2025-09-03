using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Intact.BuinessLogic.Models;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;

    public AuthController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }
}
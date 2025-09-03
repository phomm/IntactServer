using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AuthController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("check-email-status")]
    [Authorize]
    public async Task<IActionResult> CheckEmailStatus()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        return Ok(new
        {
            email = user.Email,
            isEmailConfirmed = isConfirmed
        });
    }

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
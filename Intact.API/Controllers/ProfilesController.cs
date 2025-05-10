using System.Security.Claims;
using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfilesController : ControllerBase
{
    private readonly IProfilesService _profilesService;

    public ProfilesController(IProfilesService profilesService)
    {
        _profilesService = profilesService;
    }

    [HttpGet("", Name = "GetProfiles")]
    [ProducesResponseType(typeof(IEnumerable<Profile>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilesAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _profilesService.GetAsync(Guid.Parse(userId), cancellationToken));
    }

    [HttpPost("{name}", Name = "CreateProfile")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfileAsync(string name, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var profile = await _profilesService.CreateAsync(Guid.Parse(userId), name, cancellationToken);
        return profile is null ? Conflict() : Ok(profile);
    }

    [HttpDelete("{name}", Name = "DeleteProfiles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfileAsync(string name, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _profilesService.DeleteAsync(Guid.Parse(userId), name, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
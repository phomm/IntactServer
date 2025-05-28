using System.Security.Claims;
using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ProfilesController(IProfilesService profilesService) : ControllerBase
{
    [HttpGet("", Name = "GetProfiles")]
    [ProducesResponseType(typeof(IEnumerable<Profile>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilesAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await profilesService.GetAsync(Guid.Parse(userId), cancellationToken));
    }

    [HttpPost("", Name = "CreateProfile")]
    [ProducesResponseType(typeof(Profile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfileAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var profile = await profilesService.CreateAsync(Guid.Parse(userId), name, cancellationToken);
        return profile is null ? Conflict() : Ok(profile);
    }

    [HttpDelete("{id:int}", Name = "DeleteProfile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfileAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await profilesService.DeleteAsync(Guid.Parse(userId), id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{id:int}/pick", Name = "PickProfile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PickProfileAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await profilesService.PickAsync(Guid.Parse(userId), id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
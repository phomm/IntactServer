using System.Security.Claims;
using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IProfilesService _profilesService;
    private readonly IRoomsService _roomsService;

    public RoomsController(IProfilesService profilesService, IRoomsService roomsService)
    {
        _profilesService = profilesService;
        _roomsService = roomsService;
    }
    
    [HttpGet("", Name = "GetRoom")]
    [ProducesResponseType(typeof(IEnumerable<Room>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomsAsync(CancellationToken cancellationToken)
    {
        return Ok(await _roomsService.GetAllAsync(cancellationToken));
    }
    
    [HttpPost("", Name = "CreateRoom")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRoomAsync([FromQuery] string title, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var id = await _roomsService.CreateAsync(Guid.Parse(userId), title, cancellationToken);
        return id is null ? Conflict() : Ok();
    }
    
    [HttpGet("{id:int}/availability", Name = "AvailabilityRoom")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailabilityForJoinAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await _roomsService.GetAvailabilityAsync(id, cancellationToken));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{id:int}/join", Name = "JoinRoom")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> JoinAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            return Ok(await _roomsService.JoinAsync(Guid.Parse(userId), id, cancellationToken));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost("{id:int}/exit", Name = "ExitRoom")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExitAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _roomsService.ExitAsync(Guid.Parse(userId), id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
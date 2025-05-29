using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController(IRoomsService roomsService) : ControllerBase
{
    [HttpGet("", Name = "GetRoom")]
    [ProducesResponseType(typeof(IEnumerable<Room>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomsAsync(CancellationToken cancellationToken)
    {
        return Ok(await roomsService.GetAllAsync(cancellationToken));
    }
    
    [HttpPost("", Name = "CreateRoom")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRoomAsync([FromQuery] string title, CancellationToken cancellationToken)
    {
        var id = await roomsService.CreateAsync(title, cancellationToken);
        return id is null ? Conflict() : Ok();
    }
    
    [HttpGet("{id:int}/availability", Name = "AvailabilityRoom")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailabilityForJoinAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await roomsService.GetAvailabilityAsync(id, cancellationToken));
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
        try
        {
            return Ok(await roomsService.JoinAsync(id, cancellationToken));
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
        try
        {
            await roomsService.ExitAsync(id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
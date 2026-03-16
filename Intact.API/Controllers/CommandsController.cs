using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommandsController(ICommandsService commandsService) : ControllerBase
{
    [HttpGet("", Name = "GetCommands")]
    [ProducesResponseType(typeof(IEnumerable<Command>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCommandsAsync([FromQuery] uint? offset = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var commands = await commandsService.GetCommandsAsync(offset, cancellationToken);
            return Ok(commands);
        }
        catch (ForbiddenException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: ex.Message
            );
        }
        catch (NotFoundException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: ex.Message
            );
        }
    }

    [HttpPost("", Name = "PostCommands")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostCommandsAsync([FromBody] IEnumerable<BaseCommand> commands, CancellationToken cancellationToken)
    {
        try
        {
            await commandsService.PostCommandsAsync(commands, cancellationToken);
            return Ok();
        }
        catch (BadRequestException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                detail: ex.Message
            );
        }
        catch (ForbiddenException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: ex.Message
            );
        }
        catch (NotFoundException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: ex.Message
            );
        }
    }
}

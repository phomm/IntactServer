using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtoBaseController : ControllerBase
{
    private readonly IProtoBaseService _protoBaseService;

    public ProtoBaseController(IProtoBaseService protoBaseService)
    {
        _protoBaseService = protoBaseService;
    }

    [HttpGet("", Name = "GetProtoBase")]
    [ProducesResponseType(typeof(ProtoBase), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProtoBaseAsync([FromQuery] string languagesString, CancellationToken cancellationToken)
    {
        var languages = languagesString.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return Ok(await _protoBaseService.GetProtoBaseAsync(languages, cancellationToken));
    }
}
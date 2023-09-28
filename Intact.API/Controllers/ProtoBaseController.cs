using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtoBaseController : ControllerBase
{
    private readonly ILogger<ProtoBaseController> _logger;
    private readonly IProtoBaseService _protoBaseService;

    public ProtoBaseController(ILogger<ProtoBaseController> logger, IProtoBaseService protoBaseService)
    {
        _logger = logger;
        _protoBaseService = protoBaseService;
    }

    [HttpGet("", Name = "GetProtoBase")]
    public async Task<IActionResult> GetProtoBase([FromQuery] IEnumerable<string> languageCodes, CancellationToken cancellationToken)
    {
        return Ok(await _protoBaseService.GetProtoBaseAsync(cancellationToken));
    }
}
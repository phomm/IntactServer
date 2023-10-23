using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MapsController : ControllerBase
{
    private readonly IMapsService _mapsService;

    public MapsController(IMapsService mapsService)
    {
        _mapsService = mapsService;
    }

    [HttpGet("", Name = "GetMaps")]
    [ProducesResponseType(typeof(IEnumerable<Map>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMapsAsync([FromQuery] string? languagesString, CancellationToken cancellationToken)
    {
        var languages = (languagesString ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);
        return Ok(await _mapsService.GetMapsAsync(languages, cancellationToken));
    }
}
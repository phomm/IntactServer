using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Intact.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CacheController : ControllerBase
{
    private readonly ICacheService _cacheService;

    public CacheController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpDelete("", Name = "ClearCache")]
    public async Task<IActionResult> GetMapsAsync(CancellationToken cancellationToken)
    {
        await _cacheService.ClearCache(cancellationToken);
        return Ok();
    }
}
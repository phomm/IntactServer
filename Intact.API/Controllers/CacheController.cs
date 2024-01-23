using Intact.API.Bootstrap;
using Intact.BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpDelete("clear", Name = "ClearCache")]
    //[Authorize(Policies.Admin)]
    public async Task<IActionResult> ClearCacheAsync(CancellationToken cancellationToken)
    {
        await _cacheService.ClearCache(cancellationToken);
        return Ok();
    }
}
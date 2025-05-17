using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Intact.BusinessLogic.Services;

public interface IUserAccessor
{
    public Guid GetUserId();
    public string GetUserIdStr();
}

public class UserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccessor
{
    public Guid GetUserId() =>
        Guid.Parse(GetUserIdStr());

    public string GetUserIdStr() => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new AuthenticationException("HttpContext dont return user id!");
}
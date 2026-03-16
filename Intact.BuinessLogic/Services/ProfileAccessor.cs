using System.Security.Authentication;
using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.RedisCache;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services;

public interface IProfileAccessor: IUserAccessor
{
    Task<Profile> GetProfileAsync();
    Task<int> GetProfileIdAsync();
}

public class ProfileAccessor(IHttpContextAccessor httpContextAccessor, AppDbContext context, IRedisCache redisCache) : UserAccessor(httpContextAccessor), IProfileAccessor
{
    private const string CacheSet = nameof(Profile);
    
    public async Task<Profile> GetProfileAsync()
    {
        var userId = GetUserId();
        return await redisCache.GetAsync<Profile>(CacheSet, userId.ToString())
               ?? new ProfileMapper().Map(
                   await context.Profiles
                   .Where(x => x.UserId == userId && x.State == ProfileState.Active)
                   .OrderByDescending(x => x.LastPlayed)
                   .FirstOrDefaultAsync() 
                                          ?? throw new AuthenticationException());
    }

    public async Task<int> GetProfileIdAsync() => (await GetProfileAsync()).Id;
}
using System.Security.Authentication;
using Intact.BusinessLogic.Data;
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
        return await redisCache.GetAsync<Profile>(CacheSet, GetUserIdStr())
            ?? new ProfileMapper().Map(await context.Profiles.OrderByDescending(x => x.LastPlayed).FirstOrDefaultAsync() 
                ?? throw new AuthenticationException());
    }

    public async Task<int> GetProfileIdAsync() => (await GetProfileAsync()).Id;
}
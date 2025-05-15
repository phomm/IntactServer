using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Data.RedisCache;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services;

public interface IProfilesService
{
    Task<IEnumerable<Profile>> GetAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<Profile?> CreateAsync(Guid userId, string name, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid userId, string name, CancellationToken cancellationToken);
    
    Task PickAsync(Guid userId, string name, CancellationToken cancellationToken);

    Task<Profile?> GetCurrentAsync(string userId);
}

public class ProfilesService(AppDbContext appDbContext, IRedisCache redisCache) : IProfilesService
{
    private const int ProfilesMaxCountPerUser = 5;
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IRedisCache _redisCache = redisCache;
    const string cacheSet = nameof(Profile);

    public async Task<IEnumerable<Profile>> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return ProfileMapper.Map(await _appDbContext.Profiles
            .Where(x => x.UserId == userId && x.State == ProfileState.Active)
            .OrderByDescending(x => x.LastPlayed)
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Profile?> CreateAsync(Guid userId, string name, CancellationToken cancellationToken)
    {
        if ((await GetAsync(userId, cancellationToken)).Count() < ProfilesMaxCountPerUser)
        {
            var profileDao = new ProfileDao()
            {
                Name = name,
                UserId = userId,
                CreateTime = DateTime.UtcNow,
                Rating = 0,
                State = ProfileState.Active,
                LastPlayed = DateTime.UtcNow,
                Status = "",
            };
            
            await _redisCache.AddAsync(cacheSet, userId.ToString(), profileDao);
            await _appDbContext.Profiles.AddAsync(profileDao, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return new ProfileMapper().Map(profileDao);
        }
        return null;
    }

    public async Task DeleteAsync(Guid userId, string name, CancellationToken cancellationToken)
    {
        var profileDao = await _appDbContext.Profiles.FindAsync(new object?[] { userId, name }, cancellationToken: cancellationToken);
        if (profileDao is null)
            throw new KeyNotFoundException();

        profileDao.State = ProfileState.Deleted;
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task PickAsync(Guid userId, string name, CancellationToken cancellationToken)
    {
        var profileDao = await _appDbContext.Profiles.FindAsync(new object?[] { userId, name }, cancellationToken: cancellationToken);
        if (profileDao is null)
            throw new KeyNotFoundException();
        
        profileDao.LastPlayed = DateTime.UtcNow;
        await _redisCache.AddAsync(cacheSet, userId.ToString(), profileDao);
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Profile?> GetCurrentAsync(string userId)
    {
        return _redisCache.GetAsync<Profile>(cacheSet, userId);
    }
}
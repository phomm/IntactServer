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
    
    Task DeleteAsync(Guid userId, int id, CancellationToken cancellationToken);
    
    Task PickAsync(Guid userId, int id, CancellationToken cancellationToken);
}

public class ProfilesService(AppDbContext appDbContext, IRedisCache redisCache) : IProfilesService
{
    private const int ProfilesMaxCountPerUser = 4;
    private const string CacheSet = nameof(Profile);

    public async Task<IEnumerable<Profile>> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        return ProfileMapper.Map(await GetById(userId).ToListAsync(cancellationToken: cancellationToken));
    }

    private IQueryable<ProfileDao> GetById(Guid userId)
    {
        return appDbContext.Profiles
            .Where(x => x.UserId == userId && x.State == ProfileState.Active)
            .OrderByDescending(x => x.LastPlayed);
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
            
            await appDbContext.Profiles.AddAsync(profileDao, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
            var profile = new ProfileMapper().Map(profileDao);
            await redisCache.AddAsync(CacheSet, userId.ToString(), profile);
            return profile;
        }
        return null;
    }

    public async Task DeleteAsync(Guid userId, int id, CancellationToken cancellationToken)
    {
        var profileDao = GetById(userId).FirstOrDefault(x => x.Id == id);
        if (profileDao is null)
            throw new KeyNotFoundException();

        profileDao.State = ProfileState.Deleted;
        await appDbContext.SaveChangesAsync(cancellationToken);
        if (redisCache.GetAsync<Profile>(CacheSet, userId.ToString())?.Id == profileDao.Id)
        {
            await redisCache.RemoveAsync(CacheSet, userId.ToString());
            var profile = (await GetAsync(userId, cancellationToken)).FirstOrDefault();
            if (profile != null)
                await redisCache.AddAsync(CacheSet, userId.ToString(), profile);
        }
    }

    public async Task PickAsync(Guid userId, int id, CancellationToken cancellationToken)
    {
        var profileDao = await appDbContext.Profiles.FindAsync([id], cancellationToken);
        if (profileDao is null)
            throw new KeyNotFoundException();
        
        profileDao.LastPlayed = DateTime.UtcNow;
        await appDbContext.SaveChangesAsync(cancellationToken);
        await redisCache.AddAsync(CacheSet, userId.ToString(), profileDao);
    }
}
using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services;

public interface IProfilesService
{
    Task<IEnumerable<Profile>> GetAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<Profile?> CreateAsync(Guid userId, string name, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid userId, string name, CancellationToken cancellationToken);
}

public class ProfilesService(AppDbContext appDbContext) : IProfilesService
{
    private const int ProfilesMaxCountPerUser = 5;
    private readonly AppDbContext _appDbContext = appDbContext;

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
            var profile = new ProfileDao()
            {
                Name = name,
                UserId = userId,
                CreateTime = DateTime.UtcNow,
                Rating = 0,
                State = ProfileState.Active,
                LastPlayed = DateTime.UtcNow,
                Status = "",
            };
            
            await _appDbContext.Profiles.AddAsync(profile, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return new ProfileMapper().Map(profile);
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
}
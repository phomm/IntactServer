using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Services;

public interface IRoomsService
{
    Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<int?> CreateAsync(Guid userId, string title, CancellationToken cancellationToken);
    
    Task<bool> GetAvailabilityAsync(int id, CancellationToken cancellationToken);
    
    Task<bool> JoinAsync(Guid userId, int id, CancellationToken cancellationToken);
    
    Task ExitAsync(Guid userId, int id, CancellationToken cancellationToken);
}

public class RoomsService(AppDbContext appDbContext, IProfilesService profilesService, IProfileAccessor profileAccessor): IRoomsService
{
    private const int MaxPlayers = 8;
    
    public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken)
    {
        return RoomMapper.Map(await appDbContext.Rooms.ToListAsync(cancellationToken));
    }

    public async Task<int?> CreateAsync(Guid userId, string title, CancellationToken cancellationToken)
    {
        var profileId = await profileAccessor.GetProfileIdAsync();
        var roomDao = new RoomDao()
        {
            Title = title,
            CreateDate = DateTime.UtcNow,
            Creator = profileId,
            State = RoomState.Opened,
        };
            
        await appDbContext.Rooms.AddAsync(roomDao, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
        
        await AddMemberAsync(profileId, roomDao.Id, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return roomDao.Id;
    }

    public async Task<bool> GetAvailabilityAsync(int id, CancellationToken cancellationToken)
    {
        var room = await appDbContext.Rooms.FindAsync(id);
        if (room is null)
            throw new KeyNotFoundException();

        return room.State == RoomState.Opened && await GetPlayersCount(id, cancellationToken) < MaxPlayers;
    }

    public async Task<bool> JoinAsync(Guid userId, int id, CancellationToken cancellationToken)
    {
        var profileId = await profileAccessor.GetProfileIdAsync();
        if (await GetAvailabilityAsync(id, cancellationToken) &&
            !await appDbContext.RoomMembers.Where(x => x.ProfileId == profileId).AnyAsync(cancellationToken))
        {
            await AddMemberAsync(profileId, id, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    private async Task AddMemberAsync(int profileId, int id, CancellationToken cancellationToken)
    {
        var roomMemberDao = new RoomMemberDao()
        {
            RoomId = id,
            ProfileId = profileId,
        };
        await appDbContext.RoomMembers.AddAsync(roomMemberDao, cancellationToken);
    }

    public async Task ExitAsync(Guid userId, int id, CancellationToken cancellationToken)
    {
        var profileId = await profileAccessor.GetProfileIdAsync();
        await appDbContext.RoomMembers
            .Where(x => x.ProfileId == profileId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private Task<int> GetPlayersCount(int id, CancellationToken cancellationToken) => appDbContext.RoomMembers
            .Select(x => x.RoomId == id).CountAsync(cancellationToken: cancellationToken);
}
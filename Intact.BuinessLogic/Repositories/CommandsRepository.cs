using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Repositories;

public interface ICommandsRepository
{
    Task<IEnumerable<CommandDao>> GetCommandsAsync(int roomId, uint offset, CancellationToken cancellationToken);
    
    Task<CommandDao?> GetLastCommandAsync(int roomId, CancellationToken cancellationToken);
    
    Task AddCommandsAsync(IEnumerable<CommandDao> commands, CancellationToken cancellationToken);
    
    Task<RoomDao?> GetRoomAsync(int roomId, CancellationToken cancellationToken);
    
    Task<int?> GetPlayerRoomIdAsync(int profileId, CancellationToken cancellationToken);
}

public class CommandsRepository(AppDbContext context) : ICommandsRepository
{
    public async Task<IEnumerable<CommandDao>> GetCommandsAsync(int roomId, uint offset, CancellationToken cancellationToken)
    {
        return await context.Set<CommandDao>()
            .Where(c => c.RoomId == roomId && c.QueueNumber >= offset)
            .OrderBy(c => c.QueueNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<CommandDao?> GetLastCommandAsync(int roomId, CancellationToken cancellationToken)
    {
        return await context.Set<CommandDao>()
            .Where(c => c.RoomId == roomId)
            .OrderByDescending(c => c.QueueNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddCommandsAsync(IEnumerable<CommandDao> commands, CancellationToken cancellationToken)
    {
        await context.Set<CommandDao>().AddRangeAsync(commands, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoomDao?> GetRoomAsync(int roomId, CancellationToken cancellationToken)
    {
        return await context.Rooms.FindAsync([roomId], cancellationToken);
    }

    public async Task<int?> GetPlayerRoomIdAsync(int profileId, CancellationToken cancellationToken)
    {
        var roomMember = await context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.ProfileId == profileId && !rm.Archived, cancellationToken);

        return roomMember?.RoomId;
    }
}

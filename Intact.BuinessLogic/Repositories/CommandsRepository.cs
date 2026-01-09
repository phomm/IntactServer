using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Repositories;

public class CommandsRepository(AppDbContext context) : ICommandsRepository
{
    public async Task<IEnumerable<CommandDao>> GetCommandsAsync(int roomId, int offset, CancellationToken cancellationToken)
    {
        return await context.Set<CommandDao>()
            .Where(c => c.RoomId == roomId && c.QueueNumber >= (uint)offset)
            .OrderBy(c => c.QueueNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<uint> GetNextQueueNumberAsync(int roomId, CancellationToken cancellationToken)
    {
        var lastCommand = await context.Set<CommandDao>()
            .Where(c => c.RoomId == roomId)
            .OrderByDescending(c => c.QueueNumber)
            .FirstOrDefaultAsync(cancellationToken);
        
        return lastCommand != null ? lastCommand.QueueNumber + 1 : 0;
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

    public async Task<bool> IsProfileInRoomAsync(int profileId, int roomId, CancellationToken cancellationToken)
    {
        return await context.RoomMembers
            .AnyAsync(rm => rm.ProfileId == profileId && rm.RoomId == roomId, cancellationToken);
    }
}

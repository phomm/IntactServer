using Intact.BusinessLogic.Data;
using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Repositories;

public class CommandsRepository(AppDbContext appDbContext) : ICommandsRepository
{
    public async Task<IReadOnlyList<CommandDao>> GetCommandsAsync(int roomId, int offset, CancellationToken cancellationToken)
    {
        return await appDbContext.Commands
            .Where(c => c.RoomId == roomId && c.QueueNumber >= offset)
            .OrderBy(c => c.QueueNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<CommandDao?> GetLastCommandAsync(int roomId, CancellationToken cancellationToken)
    {
        return await appDbContext.Commands
            .Where(c => c.RoomId == roomId)
            .OrderByDescending(c => c.QueueNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsPlayerInRoomAsync(int profileId, int roomId, CancellationToken cancellationToken)
    {
        return await appDbContext.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.ProfileId == profileId, cancellationToken);
    }

    public async Task<int> GetRoomPlayerCountAsync(int roomId, CancellationToken cancellationToken)
    {
        return await appDbContext.RoomMembers
            .Where(rm => rm.RoomId == roomId)
            .CountAsync(cancellationToken);
    }

    public async Task<uint> GetNextQueueNumberAsync(int roomId, CancellationToken cancellationToken)
    {
        var lastCommand = await GetLastCommandAsync(roomId, cancellationToken);
        return lastCommand == null ? 0 : lastCommand.QueueNumber + 1;
    }

    public async Task AddCommandsAsync(IEnumerable<CommandDao> commands, CancellationToken cancellationToken)
    {
        await appDbContext.Commands.AddRangeAsync(commands, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}

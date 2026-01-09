using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;

namespace Intact.BusinessLogic.Repositories;

public interface ICommandsRepository
{
    Task<IReadOnlyList<CommandDao>> GetCommandsAsync(int roomId, int offset, CancellationToken cancellationToken);
    
    Task<CommandDao?> GetLastCommandAsync(int roomId, CancellationToken cancellationToken);
    
    Task<bool> IsPlayerInRoomAsync(int profileId, int roomId, CancellationToken cancellationToken);
    
    Task<int> GetRoomPlayerCountAsync(int roomId, CancellationToken cancellationToken);
    
    Task<uint> GetNextQueueNumberAsync(int roomId, CancellationToken cancellationToken);
    
    Task AddCommandsAsync(IEnumerable<CommandDao> commands, CancellationToken cancellationToken);
}

using Intact.BusinessLogic.Data.Models;

namespace Intact.BusinessLogic.Repositories;

public interface ICommandsRepository
{
    Task<IEnumerable<CommandDao>> GetCommandsAsync(int roomId, int offset, CancellationToken cancellationToken);
    
    Task<uint> GetNextQueueNumberAsync(int roomId, CancellationToken cancellationToken);
    
    Task<CommandDao?> GetLastCommandAsync(int roomId, CancellationToken cancellationToken);
    
    Task AddCommandsAsync(IEnumerable<CommandDao> commands, CancellationToken cancellationToken);
    
    Task<RoomDao?> GetRoomAsync(int roomId, CancellationToken cancellationToken);
    
    Task<bool> IsProfileInRoomAsync(int profileId, int roomId, CancellationToken cancellationToken);
}

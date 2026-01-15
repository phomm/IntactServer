using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Repositories;

namespace Intact.BusinessLogic.Services;

public interface ICommandsService
{
    Task<IEnumerable<Command>> GetCommandsAsync(uint? offset, CancellationToken cancellationToken);
    Task PostCommandsAsync(IEnumerable<BaseCommand> commands, CancellationToken cancellationToken);
}

public class CommandsService(
    ICommandsRepository commandsRepository, 
    IProfileAccessor profileAccessor) : ICommandsService
{
    private const int MaxPlayers = 2;
    
    public async Task<IEnumerable<Command>> GetCommandsAsync(uint? offset, CancellationToken cancellationToken)
    {
        var profile = await profileAccessor.GetProfileAsync();
        
        var roomId = await commandsRepository.GetPlayerRoomIdAsync(profile.Id, cancellationToken) 
                     ?? throw new NotFoundException("Player is not in any room");
        
        // Get commands from offset
        var commandDaos = await commandsRepository.GetCommandsAsync(roomId, offset ?? 0, cancellationToken);
        return CommandMapper.Map(commandDaos);
    }

    public async Task PostCommandsAsync(IEnumerable<BaseCommand> commands, CancellationToken cancellationToken)
    {
        var profile = await profileAccessor.GetProfileAsync();
        
        var roomId = await commandsRepository.GetPlayerRoomIdAsync(profile.Id, cancellationToken)
                     ?? throw new NotFoundException("Player is not in any room");
        
        // Validate room state
        var room = await commandsRepository.GetRoomAsync(roomId, cancellationToken);
        if (room == null)
            throw new NotFoundException("Room not found");
        
        if (room.State != RoomState.InGame)
            throw new BadRequestException("Room is not in game state");
        
        var postCommandsList = commands.ToList();
        
        // Validate player numbers
        foreach (var command in postCommandsList)
        {
            if (command.PlayerIndex >= MaxPlayers)
                throw new BadRequestException("Player number out of range");
        }
        
        // Check if it's this player's turn
        var lastCommand = await commandsRepository.GetLastCommandAsync(roomId, cancellationToken);
        if (lastCommand != null)
        {
            // If last command was EndTurn from another player, then it's valid for current player
            var isOtherPlayerEndTurn = lastCommand.CommandId == CommandType.c_EndTurn && 
                                        lastCommand.ProfileId != profile.Id;
            
            // If last command was from current player and not EndTurn, can continue
            var isCurrentPlayerContinuing = lastCommand.ProfileId == profile.Id;
            
            if (!isOtherPlayerEndTurn && !isCurrentPlayerContinuing)
                throw new BadRequestException("Not your turn");
        }
        
        // Get next queue number
        var nextQueueNumber = lastCommand != null ? lastCommand.QueueNumber + 1 : 0;
        
        // Map and add commands
        var commandDaos = new List<CommandDao>();
        foreach (var postCommand in postCommandsList)
        {
            var commandDao = CommandMapper.Map(postCommand, roomId, profile.Id, nextQueueNumber);
            commandDaos.Add(commandDao);
            nextQueueNumber++;
        }
        
        await commandsRepository.AddCommandsAsync(commandDaos, cancellationToken);
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

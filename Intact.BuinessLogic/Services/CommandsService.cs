using Intact.BusinessLogic.Data.Enums;
using Intact.BusinessLogic.Data.Models;
using Intact.BusinessLogic.Mappers;
using Intact.BusinessLogic.Models;
using Intact.BusinessLogic.Repositories;

namespace Intact.BusinessLogic.Services;

public interface ICommandsService
{
    Task<IEnumerable<Command>> GetCommandsAsync(int offset, CancellationToken cancellationToken);
    
    Task<bool> PostCommandsAsync(IEnumerable<PostCommand> commands, CancellationToken cancellationToken);
}

public class CommandsService(
    ICommandsRepository commandsRepository,
    IProfileAccessor profileAccessor) : ICommandsService
{
    private const int MaxPlayersInRoom = 2;

    public async Task<IEnumerable<Command>> GetCommandsAsync(int offset, CancellationToken cancellationToken)
    {
        var profileId = await profileAccessor.GetProfileIdAsync();
        
        // Get room for the player
        var roomId = await GetPlayerRoomIdAsync(profileId, cancellationToken);
        if (roomId == null)
            throw new KeyNotFoundException("Player not in any room");

        var commands = await commandsRepository.GetCommandsAsync(roomId.Value, offset, cancellationToken);
        return CommandMapper.Map(commands);
    }

    public async Task<bool> PostCommandsAsync(IEnumerable<PostCommand> commands, CancellationToken cancellationToken)
    {
        var profileId = await profileAccessor.GetProfileIdAsync();
        var commandsList = commands.ToList();
        
        if (!commandsList.Any())
            return false;

        // Get room for the player
        var roomId = await GetPlayerRoomIdAsync(profileId, cancellationToken);
        if (roomId == null)
            throw new KeyNotFoundException("Player not in any room");

        // Validate player index is within range
        var playerCount = await commandsRepository.GetRoomPlayerCountAsync(roomId.Value, cancellationToken);
        var playerIndex = commandsList.First().PlayerIndex;
        
        if (playerIndex >= playerCount || playerIndex >= MaxPlayersInRoom)
            return false;

        // Check if all commands have the same player index
        if (commandsList.Any(c => c.PlayerIndex != playerIndex))
            return false;

        // Validate turn: check if it's this player's turn
        var lastCommand = await commandsRepository.GetLastCommandAsync(roomId.Value, cancellationToken);
        if (lastCommand != null)
        {
            // If last command was not EndTurn, it must be from the same player
            if (lastCommand.CommandId != CommandType.c_EndTurn && lastCommand.PlayerIndex != playerIndex)
                return false;
            
            // If last command was EndTurn, it must be from a different player
            if (lastCommand.CommandId == CommandType.c_EndTurn && lastCommand.PlayerIndex == playerIndex)
                return false;
        }

        // Get next queue number
        var queueNumber = await commandsRepository.GetNextQueueNumberAsync(roomId.Value, cancellationToken);

        // Create command DAOs
        var commandDaos = commandsList.Select(cmd => new CommandDao
        {
            RoomId = roomId.Value,
            ProfileId = profileId,
            PlayerIndex = cmd.PlayerIndex,
            CommandId = cmd.CommandId,
            QueueNumber = queueNumber++,
            Value = cmd.Value,
            Error = CommandError.NoError
        }).ToList();

        await commandsRepository.AddCommandsAsync(commandDaos, cancellationToken);
        return true;
    }

    private async Task<int?> GetPlayerRoomIdAsync(int profileId, CancellationToken cancellationToken)
    {
        // Find the room where this player is a member
        // This is a simplified version - you might need to adjust based on your exact room membership logic
        var rooms = await commandsRepository.GetCommandsAsync(0, 0, cancellationToken);
        
        // Check if player is in any room
        // We need to iterate through potential rooms
        // This is a placeholder - you'll need to implement proper room membership check
        for (int roomId = 1; roomId < 1000; roomId++) // Arbitrary range, adjust as needed
        {
            if (await commandsRepository.IsPlayerInRoomAsync(profileId, roomId, cancellationToken))
                return roomId;
        }
        
        return null;
    }
}

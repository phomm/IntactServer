using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class BaseCommand
{
    public ushort PlayerIndex { get; set; }
    
    public CommandType CommandId { get; set; }
    
    public string? Value { get; set; }

    public bool IsPlayerTurnCommand() => 
        PlayerTurnCommands.Contains(CommandId);

    private static CommandType[] PlayerTurnCommands = 
    [
        CommandType.c_Act, 
        CommandType.c_SetCurAct, 
        CommandType.c_ShotFailed, 
        CommandType.c_TakeUnit, 
        CommandType.c_UnitAbil, 
        CommandType.c_UnitAtk, 
        CommandType.c_UnitMove, 
        CommandType.c_UnitShot, 
        CommandType.c_UnitSpl, 
        CommandType.c_UnitSwap,
    ];    
}

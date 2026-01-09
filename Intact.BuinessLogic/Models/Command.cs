using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class Command
{
    public int ProfileId { get; init; }
    
    public ushort PlayerIndex { get; init; }
    
    public CommandType CommandId { get; init; }
    
    public uint QueueNumber { get; init; }
    
    public string? Value { get; init; }
    
    public CommandError Error { get; init; }
}

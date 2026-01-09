using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class Command
{
    public int RoomId { get; set; }
    
    public int ProfileId { get; set; }
    
    public ushort PlayerIndex { get; set; }
    
    public CommandType CommandId { get; set; }
    
    public uint QueueNumber { get; set; }
    
    public string? Value { get; set; }
    
    public CommandError Error { get; set; }
}

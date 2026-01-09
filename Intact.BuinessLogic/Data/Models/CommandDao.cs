using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public class CommandDao
{
    [Key]
    [Column(Order = 0)]
    public int RoomId { get; init; }
    
    [Key]
    [Column(Order = 1)]
    public uint QueueNumber { get; init; }
    
    public int ProfileId { get; init; }
    
    public ushort PlayerIndex { get; init; }
    
    public CommandType CommandId { get; init; }
    
    public string? Value { get; init; }
    
    public CommandError Error { get; init; } = CommandError.NoError;
}

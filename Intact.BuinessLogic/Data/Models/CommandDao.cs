using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public record CommandDao
{
    [Key]
    public required int RoomId { get; init; }
    
    public required int ProfileId { get; init; }
    
    public required ushort PlayerIndex { get; set; }
    
    public required CommandType CommandId { get; set; }
    
    [Key]
    public required uint QueueNumber { get; init; }
    
    public string? Value { get; set; }
    
    public CommandError Error { get; set; } = CommandError.NoError;
}

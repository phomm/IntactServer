using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class PostCommand
{
    public ushort PlayerIndex { get; init; }
    
    public CommandType CommandId { get; init; }
    
    public string? Value { get; init; }
}

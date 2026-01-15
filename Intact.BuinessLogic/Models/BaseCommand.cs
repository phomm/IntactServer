using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class BaseCommand
{
    public ushort PlayerIndex { get; set; }
    
    public CommandType CommandId { get; set; }
    
    public string? Value { get; set; }
}

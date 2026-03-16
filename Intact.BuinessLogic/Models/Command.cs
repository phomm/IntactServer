using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class Command: BaseCommand
{
    public uint QueueNumber { get; set; }
    
    public CommandError Error { get; set; }
}

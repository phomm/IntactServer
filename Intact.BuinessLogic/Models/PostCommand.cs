using System.ComponentModel.DataAnnotations;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public class PostCommand
{
    [Required]
    public ushort PlayerIndex { get; set; }
    
    [Required]
    public CommandType CommandId { get; set; }
    
    public string? Value { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Data.Models;

public class CommandDao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Required]
    public int RoomId { get; init; }
    
    [Required]
    public int ProfileId { get; init; }
    
    [Required]
    public ushort PlayerIndex { get; set; }
    
    [Required]
    public CommandType CommandId { get; set; }
    
    [Required]
    public uint QueueNumber { get; init; }
    
    public string? Value { get; set; }
    
    [Required]
    public CommandError Error { get; set; } = CommandError.NoError;
    
    // Navigation properties
    [ForeignKey(nameof(RoomId))]
    public RoomDao? Room { get; set; }
    
    [ForeignKey(nameof(ProfileId))]
    public ProfileDao? Profile { get; set; }
}

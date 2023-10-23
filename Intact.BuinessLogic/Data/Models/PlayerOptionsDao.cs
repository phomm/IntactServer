using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record PlayerOptionsDao
{
    [Key]
    [StringLength(16)]
    public string MapId { get; set; }
    [Key]
    public int Number { get; set; }
    public int Money { get; set; }
    public int Color { get; set; }
    public int Left { get; set; }
    public int Right { get; set; }
    public int Top { get; set; }
    public int Bottom { get; set; }
}
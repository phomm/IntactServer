using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record PlayerOptionsDao
{
    [Key]
    [StringLength(16)]
    public string MapId { get; init; }
    [Key]
    public int Number { get; init; }
    public int Money { get; init; }
    public int Color { get; init; }
    public int Left { get; init; }
    public int Right { get; init; }
    public int Top { get; init; }
    public int Bottom { get; init; }
}
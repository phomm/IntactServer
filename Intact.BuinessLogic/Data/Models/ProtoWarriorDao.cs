using Intact.BusinessLogic.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Intact.BusinessLogic.Data.Models;

public record ProtoWarriorDao : LocalizableDao
{
    [Key]
    [StringLength(16)]
    public string Id { get; init; }
    public int Number { get; init; }
    [StringLength(16)]
    public string FactionId { get; init; }
    public Force Force { get; init; }
    [StringLength(32)]
    public string AssetId { get; init; }
    public bool IsHero { get; init; }
    public bool IsRanged { get; init; }
    public bool IsMelee { get; init; }
    public bool IsBlockFree { get; init; }
    public bool IsImmune { get; init; }
    public byte InLife { get; init; }
    public byte InMana { get; init; }
    public byte InMoves { get; init; }
    public byte InActs { get; init; }
    public byte InShots { get; init; }
    public byte Cost { get; init; }
}
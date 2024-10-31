using Intact.BusinessLogic.Data.Enums;
using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record ProtoWarrior: Localizable
{
    public int Number { get; init; }
    public string FactionId { get; init; }
    public Force Force { get; init; } 
    public string AssetId { get; init; }
    public bool IsHero { get; init; }
    public bool IsRanged { get; init; }
    public bool IsMelee { get; init; }
    public bool IsBlockFree { get; init; }
    public bool IsBlockable { get; init; }
    public bool IsImmune { get; init; }
    public byte InLife { get; init; }
    public byte InMana { get; init; }
    public byte InMoves { get; init; }
    public byte InActs { get; init; }
    public byte InShots { get; init; }
    public byte Cost { get; init; }
    public List<string>? Abilities { get; set; }
    public List<string>? Spells { get; set; }
}
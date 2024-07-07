using Intact.BusinessLogic.Data.Enums;
using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record ProtoWarrior: Localizable
{
    public int Number { get; set; }
    public string Id { get; set; }
    public string FactionId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Force Force { get; set; } 
    public string AssetId { get; set; }
    public bool IsHero { get; set; }
    public bool IsRanged { get; set; }
    public bool IsMelee { get; set; }
    public bool IsBlockFree { get; set; }
    public bool IsImmune { get; set; }
    public byte InLife { get; set; }
    public byte InMana { get; set; }
    public byte InMoves { get; set; }
    public byte InActs { get; set; }
    public byte InShots { get; set; }
    public byte Cost { get; set; }
    public List<string>? Abilities { get; set; }
}
using System.Text.Json.Serialization;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public record Spell: Localizable
{
    [JsonPropertyName("spell")]
    public string Mechanics  { get; set; }

    public SpellTargetType Target { get; set; }

    public SpellUseType Use { get; set; }

    public SpellKind Kind { get; set; }

    public int Cost { get; set; }

    public int Range { get; set; }
    
    public string AssetId { get; init; }
}
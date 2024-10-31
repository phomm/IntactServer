using System.Text.Json.Serialization;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public record Spell: Localizable
{
    [JsonPropertyName("spell")]
    public string Mechanics  { get; init; }

    public SpellTarget Target { get; init; }

    public SpellUse Use { get; init; }

    public SpellKind Kind { get; init; }

    public int Cost { get; init; }

    public int Range { get; init; }
    
    public string AssetId { get; init; }
}
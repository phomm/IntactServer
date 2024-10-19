using System.Text.Json.Serialization;
using Intact.BusinessLogic.Converters;
using Intact.BusinessLogic.Data.Enums;

namespace Intact.BusinessLogic.Models;

public record Spell: Localizable
{
    public string Id { get; init; }
    
    [JsonPropertyName("spell")]
    public string Mechanics  { get; set; }
    
    [JsonConverter(typeof(JsonPrefixEnumConverter<SpellTargetType>))]
    public SpellTargetType Target { get; set; }

    [JsonConverter(typeof(JsonPrefixEnumConverter<SpellUseType>))]
    public SpellUseType Use { get; set; }

    [JsonConverter(typeof(JsonPrefixEnumConverter<SpellKind>))]
    public SpellKind Kind { get; set; }

    public int Cost { get; set; }

    public int Range { get; set; }
    
    public string AssetId { get; init; }
}
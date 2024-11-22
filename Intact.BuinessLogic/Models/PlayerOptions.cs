using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record PlayerOptions
{
    [JsonIgnore]
    public string MapId { get; init; }
    public int Number { get; init; }
    public int Money { get; init; }
    public int Color { get; init; }
    public Zone StartZone { get; set; }
}
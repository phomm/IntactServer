using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record PlayerOptions
{
    [JsonIgnore]
    public string MapId { get; set; }
    public int Number { get; set; }
    public int Money { get; set; }
    public int Color { get; set; }
    public Zone StartZone { get; set; }
}
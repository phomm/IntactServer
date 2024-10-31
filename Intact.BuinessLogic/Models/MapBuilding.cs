using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record MapBuilding
{
    [JsonIgnore]
    public string MapId { get; init; }
    public string Proto { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}
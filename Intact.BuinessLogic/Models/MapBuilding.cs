using System.Text.Json.Serialization;

namespace Intact.BusinessLogic.Models;

public record MapBuilding
{
    [JsonIgnore]
    public string MapId { get; set; }
    public string Proto { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}